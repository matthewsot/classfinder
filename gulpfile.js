var gulp = require('gulp');
var less = require('gulp-less');
var sourcemaps = require('gulp-sourcemaps');
var minifyCSS = require('gulp-minify-css');

gulp.task('default', function() {
    gulp.src('Classfinder/Classfinder/Content/Styles/**/*.less')
        .pipe(sourcemaps.init())
        .pipe(less())
        .pipe(sourcemaps.write('Classfinder/Classfinder/Content/Styles'))
        .pipe(gulp.dest(''));
});