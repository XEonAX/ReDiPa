'use strict';
var app = angular.module('ReDiPaWeb', ['ngRoute', 'ngResource', 'ngMaterial', 'ngMessages', 'angular-loading-bar', 'ngAnimate', 'chieffancypants.loadingBar']);

app.config(function ($mdThemingProvider, $routeProvider, cfpLoadingBarProvider) {
    // Configure a dark theme with primary foreground yellow

    $mdThemingProvider.theme('default')
      .primaryPalette('grey')
      .accentPalette('blue-grey');

    cfpLoadingBarProvider.includeSpinner = false;
    cfpLoadingBarProvider.includeBar = true;

    $routeProvider.when("/Tasks", {
        controller: "TasksController",
        templateUrl: "views/Tasks.html"
    });

    $routeProvider.when("/CreateTask", {
        controller: "CreateTaskController",
        templateUrl: "views/CreateTask.html"
    });

    $routeProvider.otherwise({ redirectTo: "/Tasks" });
}
);