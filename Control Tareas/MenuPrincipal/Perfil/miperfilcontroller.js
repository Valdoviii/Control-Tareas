angular.module('app').controller('miperfilcontroller', function ($scope, $http, $location, $ngConfirm, $window, $rootScope, $sessionStorage) {
    $scope.UserLoger = JSON.parse(sessionStorage.getItem('user'));
    $scope.Message = "Mi perfil";

    $scope.volvermenu = function () {
        $window.location = "../MenuPrincipal/";
    }
    $scope.refresh = function () {
        $window.location.href = "#!miperfil";
    };
    //OBTENER PERFILES
    $scope.items = 4;
    $scope.indice = 1;

    obtenerPerfiles($scope.indice, $scope.items)

    function obtenerPerfiles() {
        $http({
            method: 'GET',
            url: "https://localhost:44323/Usuario/ObtenerPerfiles1?pageindex=" + $scope.indice + "&pagesize=" + $scope.items
        }).then(
            function success(response) {
                //obtienes los registros
                $scope.listMiperfil = response.data.Resultado;
            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN OBTENER PERFILES

    //ACTUALIZAR PERFIL
    $scope.actualizarPerfil = function () {
        $ngConfirm({
            title: 'Información',
            content: "'Su perfil han sido editado con exito '" + "' Este mensaje se cerrara en 2 segundos.'",
            autoClose: 'logoutUser|2000',
            buttons: {
                logoutUser: {
                    text: 'ACEPTAR',
                    btnClass: 'btn-green',
                    action: function () {
                        $scope.editarUser();
                    }
                },

            }
        });
    }
    $scope.editarUser = function () {

        if ($scope.id_perfil == null) {
          $scope.per =  $scope.UserLoger.id_perfil;
        }
        else {
            $scope.per = $scope.id_perfil.id_perfil;
        }

        var editusuarios = {

            id_usuario: $scope.UserLoger.id_usuario,
             nombre : $scope.UserLoger.Nombre,
             apellido : $scope.UserLoger.apellido,
             correo : $scope.UserLoger.correo,
            id_perfil: $scope.per,
            pass : $scope.Micontrasena,

        };

        $http({
            method: 'POST',
            data: editusuarios,
            url: "https://localhost:44323//Usuario/editarUser"
        }).then(
            function (response) {
                $scope.refresh();
                console.log("OK");
            },
            function (error) {
                console.log("ERROR CAGAMOS");
                console.log(error);
            });
        $scope.refresh();
    };
    //FIN ACTUALIZAR PERFIL



});