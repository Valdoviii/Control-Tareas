angular.module('app').controller('usuariocontroller', function ($scope, $http, $location, $ngConfirm, $window, $rootScope, $sessionStorage) {
    $scope.Message = "Administración de Usuarios";

    //FUNCIONES COMPLEMENTARIAS
    $scope.volvermenu = function () {
        $window.location = "../MenuPrincipal/";
    }

    $scope.refresh = function () {
        $window.location.href = "#!Usuario";
    };

    $scope.AbrirModal = function (modal) {
        $(modal).modal("show");
    };

    $scope.CerrarModal = function (modal) {
        $(modal).modal("hide");
    };

    //PAGINACION
    $scope.totalItems1 = 0;
    $scope.currentPage1 = 1;
    $scope.maxSize1 = 5;

    $scope.items = 4;
    $scope.indice = 1;

    $scope.itemsRol = 4;
    $scope.indiceRol = 1;

    $scope.itemsUn = 4;
    $scope.indiceUn = 1;

    $scope.pageChanged = function () {
        $scope.listUsers1 = [];
        vistaUsuarios($scope.indice, $scope.items);
    };
   /* FIN FUNCIONES COMPLEMENTARIAS*/

    //Ejecucion de funciones
    /*  $scope.*/

    vistaUsuarios($scope.indice, $scope.items);

    obtenerPerfiles($scope.indiceRol, $scope.itemsRol);
    obtenerUnidadInterna($scope.indiceUn, $scope.itemsUn);

    //OBTENER USUARIOS
    function vistaUsuarios(indice, items) {
        $http({
            method: 'GET',
            url: "https://localhost:44323//Usuario/VistaUsuario?pageindex=" + $scope.indice + "&pagesize=" + $scope.items
        }).then(
            function success(response) {
                $scope.listUsers1 = response.data.Resultado;
                $scope.totalItems1 = response.data.Total;
            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN VISTA USUARIOS

    //EDITAR USUARIO
    $scope.id = function (id_usuario) {
        $scope.id_usuario = id_usuario;
        console.log(id_usuario);
    };
    $scope.limpiaform = function () {
            $scope.inputNombre = null,
            $scope.inputApellido = null,
            $scope.inputCorreo = null,
            $scope.inputContrasena = null,
            $scope.inputPerfil = null
    };
    $scope.editar = function () {
        $ngConfirm({
            title: 'Información',
            content: "'El usuario han sido editado con exito '" + "' Este mensaje se cerrara en 3 segundos.'",
            autoClose: 'logoutUser|3000',
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

        if ($scope.inputPerfil != null) {
            $scope.idperfil = $scope.inputPerfil.id_perfil;
        }
        if ($scope.inputUnidad != null) {
            $scope.unidad = $scope.inputUnidad.nombre
        }

        var editusuarios = {
            nombre: $scope.inputNombre,
            apellido: $scope.inputApellido,
            correo: $scope.inputCorreo,
            Contraseña: $scope.inputContrasena,
            id_usuario: id_usuario = ($scope.id_usuario),
            id_perfil: $scope.idperfil,
            UNIDADINTERNA: $scope.unidad

        };
        $http({
            method: 'POST',
            data: editusuarios,
            url: "https://localhost:44323//Usuario/editarUser"
            //url: $scope.urlUpload + '/Usuario/editarUser'
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
    //FIN EDITAR USUARIO

   //CREAR USUARIO
    $scope.crearUsuario = function () {
        $ngConfirm({
            title: 'Información',
            content: "¿ Esta seguro que desea crear el usuario ?",
            /*autoClose: 'logoutUser|3000',*/
            buttons: {
                logoutUser: {
                    text: 'ACEPTAR',
                    btnClass: 'btn-green',
                    action: function () {
                        $scope.crearusuarios();
                    }
                },

            }
        });
    }
    $scope.crearusuarios = function () {
        if ($scope.inputUnidad != null) {
            $scope.unidad = $scope.inputUnidad.nombre
        }
        $scope.inputPerfil = "2";

        var Crearusuarios = {
            nombre: $scope.inputNombre,
            apellido: $scope.inputApellido,
            correo: $scope.inputCorreo,
            perfil: $scope.inputPerfil,
            UNIDADINTERNA: $scope.unidad
        };
        $http({
            method: 'POST',
            data: Crearusuarios,
            url: "https://localhost:44323//Usuario/CrearUsuario"
            //url: $scope.urlUpload + '/Usuario/editarUser'
        }).then(
            function (response) {

                $scope.respuestUser = response.data;

                if ($scope.respuestUser == "OK") {
                    $ngConfirm({
                        title: 'Información',
                        content: "El usuario ha sido creado con exito",
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
                        content: "Usuario ya existe",
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
        $scope.refresh();
    };
   //FIN CREAR USUARIO

    //ELIMINA USUARIO
    $scope.eliminarUser = function (id_usuario) {
        console.log(id_usuario);
        $ngConfirm({
            title: 'Información',
            content: 'Esta seguro que desea eliminar Usuario',
            scope: $scope,
            buttons: {
                Aceptar: {
                    text: 'Aceptar',
                    btnClass: 'btn-blue',
                    action: function (scope, button) {
                        $scope.elimiarusuario(id_usuario);
                    }
                },
                Cancelar: {
                    text: 'Cancelar',
                    btnClass: 'btn-red',
                    action: function (scope, button) {
                        $scope.refresh();
                    }
                }
            }
        });
    }
    $scope.elimiarusuario = function (id_usuario) {
        var User = {
            ID: id_usuario,
        }
        console.log("AQUI VIENE EL OBJETO");
        console.log(User);
        $http({
            method: 'Post',
            data: User,
            url: "https://localhost:44323/Usuario/eliminarusuario"
        }).then(
            function (response) {
                if (response.data == "OK") {
                    $window.location.href = "#!Usuario";

                    console.log("OK");
                }
            

            },
            function (error) {
                console.log("ERROR CAGAMOS");
                console.log(error);
            });
        $window.location.href = "#!Usuario";
    };
    //FIN USUARIO

    //OBTENER PERFILES
    function obtenerPerfiles() {
        $http({
            method: 'GET',

            url: "https://localhost:44323//Usuario/ObtenerPerfiles1?pageindex=" + $scope.indiceRol + "&pagesize=" + $scope.itemsRol

        }).then(
            function success(response) {

                //obtienes los registros
                $scope.solicitudes2 = response.data.Resultado;
             
            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN OBTENER PERFILES

    //OBTENER UNIDAD INTERNA
    function obtenerUnidadInterna() {
        $http({
            method: 'GET',
            url: "https://localhost:44323//UnidadInterna/ObtenerUnidadesInternas?pageindex=" + $scope.indiceUn + "&pagesize=" + $scope.itemsUn + "&estado=" + "Activo"
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
    //FIN UNIDAD INTERNA







});