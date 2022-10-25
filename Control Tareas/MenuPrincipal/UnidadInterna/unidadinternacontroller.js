angular.module('app').controller('unidadinternacontroller', function ($scope, $http, $location, $ngConfirm, $window, $rootScope, $sessionStorage) {

    $scope.Message = "Unidades Internas";
    $scope.estado1 = "Activo";
    $scope.estado2 = "Inactivo";
   
    $scope.volvermenu = function () {

        $window.location = "../MenuPrincipal/";
    }
    $scope.refresh = function () {
        $window.location.href = "#!UnidadesInternas";
    };
    $scope.AbrirModal = function (modal) {
        $(modal).modal("show");
    };

    $scope.CerrarModal = function (modal) {
        $(modal).modal("hide");
    };
    //CREAR UNIDAD INTERNA
    $scope.crearUnidad= function () {
        $ngConfirm({
            title: 'Información',
            content: "'Unidad Interna a sido creado con exito '" + "' Este mensaje se cerrara en 3 segundos.'",
            autoClose: 'logoutUser|3000',
            buttons: {
                logoutUser: {
                    text: 'ACEPTAR',
                    btnClass: 'btn-green',
                    action: function () {
                        $scope.crearunidades();
                    }
                },

            }
        });
    }
    $scope.crearunidades = function () {

        $scope.fechahoy = new Date();

        var Unidad = {
            NOMBRE: $scope.inputnombre,
            FECHA_CREACION: $scope.fechahoy,
            estado: $scope.estado = "Activo"
        };
        $http({
            method: 'POST',
            data: Unidad,
            url: "https://localhost:44323//UnidadInterna/CreaUnidad"
            //url: $scope.urlUpload + '/Usuario/editarUser'
        }).then(
            function (response) {
                $scope.refresh();
                console.log("OK");
            },
            function (error) {
                console.log("ERROR ");
                console.log(error);
            });
        /*$scope.refresh();*/
    };
   //FIN CREAR UNIDAD INTERNA

    //OBTENER UNIDADES INTERNAS
    //PAGINACION
    $scope.totalItems = 0;
    $scope.currentPage = 1;
    $scope.maxSize = 4;
    $scope.items = 6;
    $scope.indice = 1;
    $scope.pageChanged = function () {
        $scope.ListUnidadInter = [];
        $scope.verActivos();
    };

    $scope.verActivos = function () {
        $scope.estado = $scope.estado1;
        $scope.ver = false;
        $scope.nover = true;
        obtenerUnidadesInternas($scope.indice, $scope.items, $scope.estado)
    }

    $scope.verInactivos = function () {
        $scope.estado = $scope.estado2;
        $scope.nover = false;
        $scope.ver = true;
        obtenerUnidadesInternas($scope.indice, $scope.items, $scope.estado)
    }

    function obtenerUnidadesInternas() {
        $http({
            method: 'GET',
            url: "https://localhost:44323//UnidadInterna/ObtenerUnidadesInternas?pageindex=" + $scope.indice + "&pagesize=" + $scope.items + "&estado=" + $scope.estado
        }).then(
            function success(response) {
                //obtienes los registros
                $scope.ListUnidadInter = response.data.Resultado;
                $scope.totalItems = response.data.Total;
            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }

    //FIN OBTENER UNIDADE INTERNAS

    //EDITAR PERFILES POR ID
    $scope.ID = function (id_unidad) {
        $scope.idUnidad = id_unidad;
    }
    $scope.EditarUnidad = function () {
        var ped = {
            id_unidad: $scope.idUnidad,
            NOMBRE: $scope.inputNombre,
            ESTADO: $scope.inputEstado,
        }
        $http({
            method: 'POST',
            data: ped,
            url: "https://localhost:44323//UnidadInterna/EditarUnidadINterna"
        }).then(
            function success(response) {
                //obtienes los registros
                $ngConfirm({
                    title: 'Información',
                    content: "'La unidad interna han sido actualizado con exito '" + "' Este mensaje se cerrara en 3 segundos.'",
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

});