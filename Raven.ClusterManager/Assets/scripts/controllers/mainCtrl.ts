/// <reference path="../main.ts" />
/*global angular:false */
'use strict';

interface SignalR {
    statsHub;
}

clusterManagerApp.controller('MainCtrl', function mainCtrl($scope, $http, $timeout) {
    var setServerCss = function (server) {
        server.cssClass = '';
        if (server.isOnline) {
            if (server.isUnauthorized) {
                server.cssClass += ' warning';
            } else {
                server.cssClass += ' success';
            }
        } else {
            server.cssClass += ' error';
        }
    };

    $http.get('/api/servers').success(function (result) {
        $scope.stats = result;
        jQuery.each(.forEach($scope.stats.servers, function (value, index, array) {
            setServerCss(value);
        });

        var statsHub = $.connection.statsHub;

        statsHub.client.addServer = function (server) {
            var stats = $scope.stats;
            setServerCss(server);
            var existingServer = stats.servers.filter(function (item) {
                return item.id == server.id;
            })[0];
            if (existingServer) {
                stats.servers;
            }
            alert('server added');
        };

        statsHub.client.addDatabase = function (database) {
            alert('db added');
        };

        $.connection.hub.start().done(function (e) {
        });






        $scope.startDiscovering = function () {
            $scope.isDiscovering = true;
            $http.get('/api/discovery/start').success(function () {
                $scope.isDiscovering = false;
            });
        };




        // timeoutPromise = $timeout($scope.getStats, 5000);
    };
    $scope.getStats();
});