/// <binding AfterBuild='default' Clean='clean' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var clean = require('gulp-clean');
var ts = require('gulp-typescript');

var libDestPath = './Scripts/libs/';
var codeDistPath = './Scripts/app';
// Delete the dist directory
gulp.task('clean', function () {
    return gulp.src(libDestPath)
        .pipe(clean())
            &&
        gulp.src(codeDistPath)
        .pipe(clean());
});

gulp.task("scriptsNStyles", function () {
    gulp.src([
            'core-js/client/*.js',
            'systemjs/dist/*.js',
            'reflect-metadata/*.js',
            'rxjs/**',
            'zone.js/dist/*.js',
            '@angular/**/bundles/*.js',
            'bootstrap/dist/js/*.js'
    ], {
        cwd: "node_modules/**"
    })
        .pipe(gulp.dest(libDestPath));
});

gulp.task('moveCss', [], function () {
    gulp.src("./Angular/src/**/*.css")
        .pipe(gulp.dest(codeDistPath));
});

gulp.task('moveHtml', [], function () {
    gulp.src("./Angular/src/**/*.html")
        .pipe(gulp.dest(codeDistPath));
});

gulp.task('default', ['scriptsNStyles', 'moveCss', 'moveHtml']);