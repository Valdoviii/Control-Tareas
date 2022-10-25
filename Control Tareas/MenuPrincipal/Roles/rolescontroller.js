angular.module('app').controller('rolescontroller', function ($scope, $http, $location, $ngConfirm, $window, $rootScope, $sessionStorage) {

    $scope.Message = "Perfiles de usuarios";
    $scope.fechahoy = new Date();

    $scope.estado1 = "Activo";
    $scope.estado2 = "Inactivo";

    //PAGINACION
    $scope.totalItems = 0;
    $scope.currentPage = 1;
    $scope.maxSize = 4;

    //$scope.items indica la cantidad de registros mostrara por pagina
    $scope.items = 3;
    $scope.indice = 1;

    $scope.pageChanged = function () {
        $scope.perfilesSol = [];
        $scope.verActivos();
    };

    $scope.verActivos = function () {
        $scope.estado = $scope.estado1;
        $scope.ver = false;
        $scope.nover = true;
        obtenerPerfiles($scope.indice, $scope.items, $scope.estado)
    }

    $scope.verInactivos = function () {
        $scope.estado = $scope.estado2;
        $scope.nover = false;
        $scope.ver = true;
        obtenerPerfiles($scope.indice, $scope.items, $scope.estado)
    }

    $scope.volvermenu = function () {
        $window.location = "../MenuPrincipal/";
    }
    $scope.refresh = function () {
        $window.location.href = "#!Roles";
    };
    $scope.AbrirModal = function (modal) {
        $(modal).modal("show");
    };
    $scope.CerrarModal = function (modal) {
        $(modal).modal("hide");
    };

    //OBTENER PERFILES
    function obtenerPerfiles() {
        $http({
            method: 'GET',
            url: "https://localhost:44323//Usuario/ObtenerPerfiles?pageindex=" + $scope.indice + "&pagesize=" + $scope.items + "&estado=" + $scope.estado
        }).then(
            function success(response) {
                //obtienes los registros
                $scope.perfilesSol = response.data.Resultado;
                $scope.totalItems = response.data.Total;
            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN OBTENER PERFILES

    //EDITAR PERFILES POR ID
    $scope.IDPer = function (id_perfil) {
        $scope.idperfiles = id_perfil;
    }
    $scope.EditarPerfilesID = function () {
        var ped = {
            id_perfil: $scope.idperfiles,
            nombre: $scope.inputNombre,
            ESTADO: $scope.inputEstado,
        }
        $http({
            method: 'POST',
            data: ped,
            url: "https://localhost:44323//Usuario/EditarPErfil"
        }).then(
            function success(response) {
                //obtienes los registros
                $ngConfirm({
                    title: 'Información',
                    content: "'El perfil han sido actualizado con exito '" + "' Este mensaje se cerrara en 3 segundos.'",
                    autoClose: 'logoutUser|3000',
                    buttons: {
                        logoutUser: {
                            text: 'ACEPTAR',
                            btnClass: 'btn-green',
                            action: function () {
                                $scope.refresh();
                            }
                        },
                    }
                });

            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN EDITAR PERFILES

    $scope.crearPerfil = function () {
        var per = {
            NOMBRE: $scope.inputNombre,
            FECHACREACION: $scope.fechahoy,
            ESTADO: $scope.estado = "Activo",
        }
        $http({
            method: 'POST',
            data: per,
            url: "https://localhost:44323//Usuario/CreacionPerfil"
        }).then(
            function (response) {
                $scope.refresh();
                console.log("OK");
            },
            function (error) {
                console.log("ERROR ");
                console.log(error);
            });
    }

   


});