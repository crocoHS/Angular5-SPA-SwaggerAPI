/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var sass = require('gulp-sass');
var err = null;

gulp.task('sass', function () {
    console.log('Building scss files...');
    return gulp.src('wwwroot/dist/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('wwwroot/dist/'));
});

gulp.task('sass:watch', function () {
    var watcher = gulp.watch('wwwroot/dist/*.scss', ['sass']);
    watcher.on('change', function (event) {
        console.log('File ' + event.path + ' was ' + event.type);
    });
});


gulp.task('default', ['task_one', 'task_two'], function () {
    var watcher = gulp.watch('ClientApp/app/Components/**/*.ts', ['uglify', 'reload']);
    watcher.on('change', function (event) {
        console.log('File ' + event.path + ' was ' + event.type + ', running tasks...');
    });
});

// takes in a callback so the engine knows when it'll be done
gulp.task('task_one', function (cb) {
    // do stuff - async or otherwise
    cb(err); // if err is not null and not undefined, the run will stop, and note that it failed
});

// identifies a dependent task must be complete before this one begins
gulp.task('task_two', ['task_one'], function () {
    // task 'one' is done now
});

gulp.task('uglify', function () {
    // uglify
});

gulp.task('reload', function () {
    // reload
});
