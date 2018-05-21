import menpo.io as mio
import menpo
import menpo3d.io as m3io
import numpy as np
import os
import shutil

from menpofit.aam import load_balanced_frontal_face_fitter
from menpo.shape import TexturedTriMesh
from menpo3d.morphablemodel import ColouredMorphableModel
from menpodetect.dlib import load_dlib_frontal_face_detector
from menpofit.dlib import DlibWrapper

from menpo3d.morphablemodel.fitter import LucasKanadeMMFitter
from menpo.transform import image_coords_to_tcoords

from PIL import Image
from pathlib import Path

detect = load_dlib_frontal_face_detector()
aam_fitter = load_balanced_frontal_face_fitter()


shape_model, landmarks = mio.import_pickle("Child Customisation/pkls/children_under7.pkl")
texture_model = mio.import_pickle('Child Customisation/pkls/fast_dsift.pkl')
tcoords, bcoords_img, tri_index_img = mio.import_pickle('Child Customisation/pkls/unwrapped_template_barycentrics.pkl')

def extract_texture(mesh_in_image, background):

    TI = tri_index_img.as_vector()
    BC = bcoords_img.as_vector(keep_channels=True).T

    sample_points_3d = mesh_in_image.project_barycentric_coordinates(BC, TI)
    
    texture = bcoords_img.from_vector(background.sample(sample_points_3d.points[:, :2]))
    
    return texture

def transform(result, mesh):
    return result._affine_transforms[-1].apply(result.camera_transforms[-1].apply(mesh))

def fit_model():

    # # move all user related image files in own folder
    # newdir = os.path.join(os.getcwd(), "User_Image")
    # delete_folder_contents(newdir)

    # newfile = os.path.join(newdir, "original_image.jpg")
    # os.rename(os.getcwd(), newfile)

    # import image
    image = mio.import_images("Child Customisation/Assets/Resources/User_Image/*.jpg")[0]

    # the morphable model
    mm = ColouredMorphableModel(shape_model, texture_model, landmarks, 
                            holistic_features=menpo.feature.fast_dsift, diagonal=185)
    
    # bb: bounding_box
    bb = detect(image)[0]

    fitter = LucasKanadeMMFitter(mm, n_shape=200, n_texture=200, n_samples=8000, n_scales=1)

    # initial_shape: An image of points indicating landmarks on the face
    initial_shape = aam_fitter.fit_from_bb(image, bb).final_shape

    result = fitter.fit_from_shape(image, initial_shape, max_iters=30, 
                               camera_update=True, 
                               focal_length_update=False, 
                               reconstruction_weight=1,
                               shape_prior_weight=1e7,
                               texture_prior_weight=1.,
                               landmarks_prior_weight=1e5,
                               return_costs=False, 
                               init_shape_params_from_lms=False)
    # The mesh in image coordinates
    mesh_in_img = transform(result, result.final_mesh)

    # A flattened image showing the face extracted from the orginal image
    uv = extract_texture(mesh_in_img, image)

    textured_mesh = TexturedTriMesh(result.final_mesh.points,
                    tcoords=image_coords_to_tcoords(uv.shape).apply(tcoords).points,
                    texture=uv,
                    trilist=result.final_mesh.trilist)
    
    # return textured_mesh 
    m3io.export_textured_mesh(textured_mesh, 'Child Customisation/Assets/Resources/User_Image/texture.obj', extension='obj', overwrite=True)

def save_locally(filename):
    newdir = os.path.join(os.getcwd(), "Child Customisation/Assets/Resources/User_Image")
    delete_folder_contents(newdir)

    newfile = os.path.join(newdir, "original_image.jpg")
    shutil.copy(filename, newfile)

def delete_folder_contents(folder):
    for file in os.listdir(folder):
      file_path = os.path.join(folder, file)
      try:
          if os.path.isfile(file_path):
              os.unlink(file_path)
      except Exception as e:
          print(e)

####### Example usage of functions

# An external example image of the face to be mapped to the model
# image = mio.import_images('Images/*')[0]

# A 3d mesh with the generated texture
# textured_mesh = fit_model(image)                                            

# Save the mesh to file
# m3io.export_textured_mesh(textured_mesh, 'Images/texture.obj', extension='obj', overwrite=True)
