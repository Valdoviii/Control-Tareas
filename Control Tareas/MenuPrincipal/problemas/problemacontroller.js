angular.module('app').controller('problemacontroller', function ($scope, $http, $location, $ngConfirm, $window, $rootScope, $sessionStorage) {
    $scope.Users = JSON.parse(sessionStorage.getItem('user'));
    $scope.Message = "Registro problemas";
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
        $scope.ListProblema = [];
        $scope.verActivos();
    };

    $scope.verActivos = function () {
        $scope.estado = $scope.estado1;
        $scope.ver = false;
        $scope.nover = true;
        obtenerProblemas($scope.indice, $scope.items, $scope.estado)
    }

    $scope.verInactivos = function () {
        $scope.estado = $scope.estado2;
        $scope.nover = false;
        $scope.ver = true;
        obtenerProblemas($scope.indice, $scope.items, $scope.estado)
    }

    $scope.volvermenu = function () {
        $window.location = "../MenuPrincipal/";
    }
    $scope.refresh = function () {
        $window.location.href = "#!Problema";
    };
    $scope.AbrirModal = function (modal) {
        $(modal).modal("show");
    };
    $scope.CerrarModal = function (modal) {
        $(modal).modal("hide");
    };

    $scope.crearProblema = function () {
        $scope.propietario = $scope.Users.Nombre +" " +$scope.Users.apellido;
        var per = {
            ID_TAREA: $scope.idTareas,
            NOMBRE: $scope.inputNombre,
            FECHA_CREACION: $scope.fechahoy,
            DESCRIPCION: $scope.inputDescripcion,
            CRITICIDAD: $scope.inputCriticidad,
            PROPIETARIO: $scope.propietario,
            ESTADO: $scope.estados = "Activo"
        }
        $http({
            method: 'POST',
            data: per,
            url: "https://localhost:44323//Problema/CreacionProblema"
        }).then(
            function (response) {
                $scope.respuestUser = response.data;
                if ($scope.respuestUser == "OK") {
                    $ngConfirm({
                        title: 'Información',
                        content: "El problema ha sido creado con exito",
                        /*autoClose: 'logoutUser|3000',*/
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
                }
                if ($scope.respuestUser == "NOK") {
                    $ngConfirm({
                        title: 'Información',
                        content: "No fue posible crear el registro de problema",
                        /*autoClose: 'logoutUser|3000',*/
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
                }
                console.log("OK");
            },
            function (error) {
                console.log("ERROR ");
                console.log(error);
            });
    }

    //OBTENER problema
    function obtenerProblemas() {
        $http({
            method: 'GET',
            url: "https://localhost:44323//Problema/ObtenerProblemas?pageindex=" + $scope.indice + "&pagesize=" + $scope.items + "&estado=" + $scope.estado
        }).then(
            function success(response) {
                //obtienes los registros
                $scope.ListProblema = response.data.Resultado;
                $scope.totalItems = response.data.Total;
            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN OBTENER problema

    //OBTENER PROBLEMA SELECCIONADA
    $scope.vermiProblema = function (id_problemas) {
        $http({
            method: 'GET',
            url: "https://localhost:44323//Problema/ObtenerProblemaEdit?pageindex=" + $scope.indice + "&pagesize=" + $scope.items + "&id_problemas=" + id_problemas
        }).then(
            function success(response) {
                $scope.listProblemaEdit = response.data.Resultado;
                $scope.totalItems = response.data.Total;
            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN VISTA TAREAS POR USUARIO


    //EDITAR problema POR ID
    $scope.ID = function (id_problemas) {
        $scope.idproblema = id_problemas;
        $scope.vermiProblema(id_problemas);
    }
    $scope.EditarProblema = function (reg) {
        var ped = {
            ID_PROBLEMA: reg.id_problemas,
            NOMBRE: reg.nombre,
            DESCRIPCION: reg.descripcion,
            CRITICIDAD: reg.criticidad,
            ESTADO: reg.estado
        }
        $http({
            method: 'POST',
            data: ped,
            url: "https://localhost:44323//Problema/editarProblema"
        }).then(
            function success(response) {
                //obtienes los registros
                $scope.respuestUser = response.data;
                if ($scope.respuestUser == "OK") {
                    $ngConfirm({
                        title: 'Información',
                        content: "El problema ha sido editado con exito",
                        /*autoClose: 'logoutUser|3000',*/
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
                }
                if ($scope.respuestUser == "NOK") {
                    $ngConfirm({
                        title: 'Información',
                        content: "No fue posible editar el registro de problema",
                        /*autoClose: 'logoutUser|3000',*/
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
                }
            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN EDITAR problema

    

   


});