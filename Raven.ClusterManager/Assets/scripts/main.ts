/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/angularjs/angular.d.ts" />
/// <reference path="typings/underscore/underscore.d.ts" />
/// <reference path="typings/signalr/signalr.d.ts" />
/*global angular:false */
'use strict';

var clusterManagerApp = angular.module('ClusterManagerApp', ['ui.bootstrap', 'ui.bootstrap.tpls']);

clusterManagerApp.config(function ($routeProvider, $locationProvider) {
    $routeProvider.when('/servers/*id', {
        templateUrl: '/views/serverExplorer.html',
        controller: 'ServerExplorerCtrl'
    });
    $routeProvider.when('/servers', {
        templateUrl: '/views/servers.html',
        controller: 'ServersCtrl'
    });
    
    $routeProvider.when('/replication', {
        templateUrl: '/views/replication.html',
        controller: 'ReplicationCtrl'
    });

    $routeProvider.when('/', {
        templateUrl: '/views/main.html',
        controller: 'MainCtrl'
    });
    $routeProvider.otherwise({ redirectTo: '/' });
    
    $locationProvider.html5Mode(true);
});