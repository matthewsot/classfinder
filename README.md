Classfinder
===========

A class sharing website for students in Lynbrook High School


## Building & Running Classfinder
You can host Classfinder on any server that supports ASP.NET.

Before hosting it, though, you'll need to compile the .less files and minify the .css files. To help with this, there's a handy dandy gulpfile that you can use to do it in seconds.

First you'll need to [install Node](http://nodejs.org/), then open a command prompt in the classfinder root folder. Run the following to install gulp and everything else we'll be using:

```
npm install
```

Now that you've got node and gulp installed, each time you pull from the repo just run:

```
gulp
```

in a command prompt and it should automagically create .css, .css.map, and .min.css files where appropriate in the solution.