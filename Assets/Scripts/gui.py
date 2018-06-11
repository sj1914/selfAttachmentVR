#!/Users/summerjones/miniconda3/bin/python

from tkinter import *
from tkinter import filedialog
from tkinter.ttk import Progressbar
from tkinter import ttk
import tkinter as tk
from image_to_texture import fit_model, save_locally
from tkinter import Tk, RIGHT, BOTH, RAISED, TOP
from tkinter.ttk import Frame, Button, Style


class SAT(Frame):

    filename = ""

    def __init__(self):
        super().__init__()   
         
        self.initUI()

    def initUI(self):
      
        self.master.title("Self Attachment VR")
        self.pack(fill=BOTH, expand=True)

        frame1 = Frame(self)
        frame1.pack(fill=BOTH, expand=True)
        
        intro1 = Label(frame1, text="Welcome to Self Attachment in VR!", bg="#E8E8E8")
        intro1.pack(side=LEFT, anchor=N, padx=5)      

        frame2 = Frame(self)
        frame2.pack(fill=X)
        
        self.info = Label(frame2, text="Please upload an image of your child self to proceed.", bg="#E8E8E8")
        self.info.pack(side=LEFT, padx=5)         
       
        frame3 = Frame(self)
        frame3.pack(fill=X)      

        self.entry = Entry(frame3, width=31)
        self.entry.pack(side=LEFT, padx=5, pady=5)

        browseButton = Button(frame3, text="Browse...", command=self.open_file)
        browseButton.pack(fill=X, padx=5, expand=True)  

        frame5 = Frame(self)
        frame5.pack(fill=X)
        
        self.info2 = Label(frame5, text="Application will launch automatically when upload complete.", bg="#E8E8E8")
        self.info2.pack(side=LEFT, padx=5)     

        frame4 = Frame(self)
        frame4.pack(fill=X)

        # closeButton = Button(frame4, text="Close", command=self.quit)
        # closeButton.pack(side=RIGHT, padx=5, pady=5)
        uploadButton = Button(frame4, text="Upload", command=self.upload)
        uploadButton.pack(side=RIGHT, padx=5, pady=5)


    def open_file(self):
        self.filename = filedialog.askopenfilename(initialdir = "/",title = "Select file",filetypes = (("jpeg files","*.jpg"),("all files","*.*")))
        print(self.filename)
        self.entry.delete(0, END)
        self.entry.insert(0, self.filename)

    def upload(self):
        if not self.filename:
            # self.info.configure(text="No File Selected.")
            # self.info.configure(bg="red")
            print("no file selected")
            return
        # self.info.configure(text="Application will launch automatically when upload complete.")
        # self.info.configure(bg="green")
        save_locally(self.filename)
        textured_mesh = fit_model() 
        root.destroy()




def center(window):
    window.update_idletasks()
    w = window.winfo_screenwidth()
    h = window.winfo_screenheight()
    size = tuple(int(_) for _ in window.geometry().split('+')[0].split('x'))
    x = w/2 - size[0]/2
    y = h/2 - size[1]/2
    window.geometry("%dx%d+%d+%d" % (size + (x, y)))

root = Tk()
root.geometry("400x150")
center(root)
app = SAT()
root.mainloop() 