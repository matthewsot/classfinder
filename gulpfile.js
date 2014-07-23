var gulp = require('gulp');
var less = require('gulp-less');
var sourcemaps = require('gulp-sourcemaps');
var minifyCSS = require('gulp-minify-css');
var rename = require("gulp-rename");
var gulpignore = require('gulp-ignore');

gulp.task('compile-less', function () {
    gulp.src('./Classfinder/Classfinder/Content/Styles/**/*.less')
        .pipe(sourcemaps.init())
        .pipe(less())
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest('./Classfinder/Classfinder/Content/Styles'));
});

gulp.task('minify-css', function() {
    gulp.src('./Classfinder/Classfinder/Content/Styles/**/*.css')
        .pipe(gulpignore.exclude(/.*\.min\.css$/, minifyCSS()))
        .pipe(rename({
            extname: ".min.css"
        }))
        .pipe(gulp.dest('./Classfinder/Classfinder/Content/Styles'));
});

gulp.task('default', [ 'compile-less', 'minify-css' ]);