angular.module('app').run(function ($rootScope, $sessionStorage) {



});

//RUTAS DE CADA MODULO
angular.module('app').config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'Menu/Menu.html',
            controller: 'menucontroller'
        })
        .when('/Menu', {
            templateUrl: 'Menu/Menu.html',
            controller: 'menucontroller'
        })


        .when('/Usuario', {
            templateUrl: 'Usuario/usuario.html',
            controller: 'usuariocontroller'
        })

        .when('/Roles', {
            templateUrl: 'Roles/roles.html',
            controller: 'rolescontroller'
        })

        .when('/RegistroTareas', {
            templateUrl: 'RegistroTareas/registroTareas.html',
            controller: 'registroTareascontroller'
        })

        .when('/miperfil', {
            templateUrl:'Perfil/miperfil.html',
            controller:'miperfilcontroller'

        })
        .when('/problema', {
            templateUrl: 'problemas/Problema.html',
            controller: 'problemacontroller'
        })

        .when('/UnidadesInternas', {
            templateUrl: 'UnidadInterna/unidadinterna.html',
            controller: 'unidadinternacontroller'

        })
        .otherwise({
            redirectTo: '/'
        });
});



