module.exports = function(grunt) {
    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        
        less: {
            options: {
                paths: [
                    /*"Classfinder/Classfinder/Content/Styles",
                    "Classfinder/Classfinder/Content/Styles/Site",
                    "Classfinder/Classfinder/Content/Styles/Site/Default",
                    "Classfinder/Classfinder/Content/Styles/Site/Home",
                    "Classfinder/Classfinder/Content/Styles/Site/Manage",
                    "Classfinder/Classfinder/Content/Styles/Site/Welcome",*/
                ],
                cleancss: false,
                sourceMap: true,
                modifyVars: {
                    imgPath: '"http://mycdn.com/path/to/images"',
                    bgColor: 'red'
                }
            },
            dynamic_mappings: {
                files: [
                    {
                        expand: true,     // Enable dynamic expansion.
                        cwd: 'Classfinder/Classfinder/Content/Styles/',     // Src matches are relative to this path.
                        src: ['**/*.less'], // Actual pattern(s) to match.
                        dest: 'Classfinder/Classfinder/Content/Styles/',   // Destination path prefix.
                        ext: '.css',   // Dest filepaths will have this extension.
                        extDot: 'first'   // Extensions in filenames begin after the first dot
                    },
                ],
            },
        },
        cssmin: {
            minify: {
                expand: true,
                cwd: 'Classfinder/Classfinder/Content/Styles/',
                src: ['**/*.css', '!**/*.min.css'],
                dest: 'Classfinder/Classfinder/Content/Styles/',
                ext: '.min.css'
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    
    // Default task(s).
    grunt.registerTask('default', ['less','cssmin']);
};