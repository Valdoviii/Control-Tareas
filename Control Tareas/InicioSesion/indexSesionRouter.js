angular.module('app').run(function ($rootScope, $sessionStorage) {
    $sessionStorage.$reset();

});

angular.module('app').config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'Login/Login.html',
            controller: 'logincontroller'
        })

        .when('/Login', {
            templateUrl: 'Login/Login.html',
            controller: 'logincontroller'
        })

        .when('/Recuperar', {
            templateUrl: 'RecuperarCuenta/Recuperar.html',
            controller: 'Recuperarcontroller'
        })



        .otherwise({
            redirectTo: '/'
        });
});



