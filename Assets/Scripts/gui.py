#!/Users/summerjones/miniconda3/bin/python

from tkinter import *
from tkinter import filedialog
from tkinter.ttk import Progressbar
from tkinter import ttk
import tkinter as tk
from image_to_texture import fit_model, save_locally


class App:

    filename = ""

    def __init__(self, master):

        frame = Frame(master, bd=10)
        frame.pack()

        self.intro = Message(frame,text="Upload an image of your child self.")
        self.intro.pack(side=TOP)

        self.quit = Button(frame, text="Cancel", fg="red", command=frame.quit)
        self.quit.pack(side=LEFT)

        self.browse = Button(frame, text="Browse", command=self.open_file)
        self.browse.pack(side=LEFT)

        self.upload = Button(master, text="Upload", command=self.upload)
        self.upload.pack(side=TOP)

        self.input = makeentry(master, "File Location:", 10)
        self.input.pack(side=LEFT)

    def open_file(self):
        self.filename = filedialog.askopenfilename(initialdir = "/",title = "Select file",filetypes = (("jpeg files","*.jpg"),("all files","*.*")))
        print(self.filename)
        self.input.delete(0, END)
        self.input.insert(0, self.filename)

    def upload(self):
        if not self.filename:
            print("no file selected")
        save_locally(self.filename)
        textured_mesh = fit_model() 
         

def makeentry(parent, caption, width=None, **options):
    Label(parent, text=caption).pack(side=LEFT)
    entry = Entry(parent, **options)
    if width:
        entry.config(width=width)
    entry.pack(side=LEFT)
    return entry

def center(window):
    window.update_idletasks()
    w = window.winfo_screenwidth()
    h = window.winfo_screenheight()
    size = tuple(int(_) for _ in window.geometry().split('+')[0].split('x'))
    x = w/2 - size[0]/2
    y = h/2 - size[1]/2
    window.geometry("%dx%d+%d+%d" % (size + (x, y)))

# def launchunity():
#     os.system('unity.exe')

root = Tk()
center(root)
root.title("Upload Image")
# root.geometry('200x200')

app = App(root)



root.mainloop()
root.destroy() # optional; see description below