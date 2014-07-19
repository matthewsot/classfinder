module.exports = function(grunt) {
    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        
        //First we compile all the LESS files to .css
        less: {
            options: {
                cleancss: false, //we'll minify it later, so that it will output a separate file
                sourceMap: true,
            },
            dynamic_mappings: {
                files: [{
                    expand: true,
                    cwd: 'Classfinder/Classfinder/Content/Styles/',
                    src: ['**/*.less'],
                    dest: 'Classfinder/Classfinder/Content/Styles/',
                    ext: '.css',
                    extDot: 'first'
                }],
            },
        },
        
        //Then minify them to .min.css
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