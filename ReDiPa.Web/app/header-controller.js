'use strict';
app.controller('HeaderController', function ($scope, $mdDialog, $mdToast, $window) {
    $scope.showLogin = function (ev) {
        $mdDialog.show({
            controller: loginDialogController,
            templateUrl: 'views/LoginDialog.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true
        })
        .then(function login(user) {
            $mdToast.show($mdToast.simple().content('You tried logging in with "' + user.userid + ' , ' + user.password + '".'));
        }, function cancel() {
            $mdToast.show($mdToast.simple().content('You cancelled the dialog.'));
        });
    };
    $window.loading_screen.finish();
});
function loginDialogController($scope, $mdDialog) {
    $scope.hide = function () {
        $mdDialog.hide();
    };
    $scope.cancel = function () {
        $mdDialog.cancel();
    };
    $scope.answer = function (answer) {
        $mdDialog.hide(answer);
    };
    $scope.login = function (user) {
        $mdDialog.hide(user);
    };

};