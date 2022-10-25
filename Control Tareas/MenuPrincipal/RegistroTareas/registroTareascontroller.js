angular.module('app').controller('registroTareascontroller', function ($scope, $http, $location, $ngConfirm, $window, $rootScope, $sessionStorage) {
    $scope.Users = JSON.parse(sessionStorage.getItem('user'));

    $scope.Message = "Registro tareas";
    $scope.titulomisregis = "Mi Registro de Tarea";
    $scope.fechahoy = new Date();

    $scope.UserLog = JSON.parse(sessionStorage.getItem('user'));

    $scope.ingresado = "primary";
    $scope.proceso = "warning";
    $scope.finalizado = "success";
    $scope.vencido = "danger";

    $scope.color = "";
    $scope.porcentaje = "";

    /*position: absolute; width: 50%; height: 100 %; background - color: #4CAF50;*/

    $scope.estado1 = "Activo";
    $scope.estado2 = "Inactivo";

    $scope.volvermenu = function () {
        $window.location = "../MenuPrincipal/";
    };
    $scope.refresh = function () {
        $window.location.href = "#!RegistroTareas";
    };
    $scope.AbrirModal = function (modal) {
        $(modal).modal("show");
    };

    $scope.CerrarModal = function (modal) {
        $(modal).modal("hide");
    };

    //PAGINACION
    $scope.totalItems1 = 0;
    $scope.totalItems = 0;
    $scope.currentPage = 1;
    $scope.maxSize = 5;
    $scope.maxSize1 = 5;

    //$scope.items indica la cantidad de registros mostrara por pagina
    $scope.itemsPro = 0;
    $scope.indicePro = 1;

    $scope.itemsUser = 0;
    $scope.indiceUser = 1;

    $scope.itemsTareas = 0;
    $scope.indiceTareas = 1;

    $scope.pageChanged = function () {
        $scope.listTareas = [];
        obtenerTareas();
    };

    $scope.verActivos = function () {
        $scope.estado = $scope.estado1;
        $scope.ver = false;
        $scope.nover = true;

        if ($scope.UserLog.id_usuario == 1) {
            obtenerTareas($scope.indiceTareas, $scope.itemsTareas, $scope.estado)
        }
        if ($scope.UserLog.id_usuario != 1) {
            ObtenerTareasUserLogeado()
        }
    }

    $scope.verInactivos = function () {
        $scope.estado = $scope.estado2;
        $scope.nover = false;
        $scope.ver = true;

        if ($scope.UserLog.id_usuario == 1) {
            obtenerTareas($scope.indiceTareas, $scope.itemsTareas, $scope.estado)
        }
        if ($scope.UserLog.id_usuario != 1) {
            ObtenerTareasUserLogeado()
        }
    }

    //$scope.vermitarea = function (id_tarea) {
    //   var idTarea= id_tarea;
    //}

    //Ejecucion de funciones
    vistaUsuarios($scope.indiceUser, $scope.itemsUser);

    //OBTENER TAREAS POR USUARIO
    $scope.vermitarea = function (id_tareas) {
        $scope.idper = id_tareas

        $http({
            method: 'GET',

            url: "https://localhost:44323//Tareas/ObtenerTareasUsuario?id_tarea=" + $scope.idper
        }).then(
            function success(response) {
                $scope.listTareasUseer = response.data;
                if ($scope.listTareasUseer[0].estado == "Ingresado") {
                    $scope.porcentaje = "20%";
                    $scope.myObj = { "position": "absolute", "background-color": "#4CAF50", "height": "100%", "width": "20%" }
                }
                if ($scope.listTareasUseer[0].estado == "Proceso") {
                    $scope.porcentaje = "50%";
                    $scope.myObj = { "position": "absolute", "background-color": "#4CAF50", "height": "100%", "width": "50%" }
                }
                if ($scope.listTareasUseer[0].estado == "Finalizado") {
                    $scope.porcentaje = "100%";
                    $scope.myObj = { "position": "absolute", "background-color": "#4CAF50", "height": "100%", "width": "100%" }
                }
                if ($scope.listTareasUseer[0].estado == "Vencido") {
                    $scope.myObj = { "position": "absolute", "background-color": "#FE0404", "height": "100%", "width": "100%" }
                    $scope.porcentaje = "VENCIDO";
                }
                $scope.LstTareaUser = response.data[0].LstTareaUser;
                $scope.totalItems = response.data.Total;
            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN VISTA TAREAS POR USUARIO

    //OBTENER TAREAS PARA ADMIN
    function obtenerTareas() {
        $http({
            method: 'GET',
            url: "https://localhost:44323//Tareas/ObtenerTareas?pageindex=" + $scope.indiceTareas + "&pagesize=" + $scope.itemsTareas + "&estado=" + $scope.estado
        }).then(
            function success(response) {
                $scope.listTareas = response.data.Resultado;
                $scope.totalItems1 = response.data.Total;

            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN VISTA TAREAS PARA ADMIN


    //OBTENER TAREAS POR USUARIO LOGEADO
    function ObtenerTareasUserLogeado() {


        $http({
            method: 'GET',
            url: "https://localhost:44323//Tareas/ObtenerTareasUserLog?pageindex=" + $scope.indiceTareas + "&pagesize=" + $scope.itemsTareas + "&estado=" + $scope.estado + "&id_usuario=" + $scope.UserLog.id_usuario
        }).then(
            function success(response) {

                $scope.listTareas = response.data.Resultado;
                $scope.totalItems = response.data.Total;

            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN VISTA TAREAS POR USUARIO LOGEADO


    //OBTENER USUARIOS LOOKUP
    function vistaUsuarios() {
        $http({
            method: 'GET',
            url: "https://localhost:44323//Usuario/VistaUsuario?pageindex=" + $scope.indiceUser + "&pagesize=" + $scope.itemsUser

        }).then(
            function success(response) {
                $scope.listaUsers = response.data.Resultado;

            }, function (error) {
                console.log("ERROR RESPONSE");
                console.log(error);
            });
    }
    //FIN VISTA USUARIOS


    //CREACION DE TAREAS (los estado de las tareas son:  'Ingresado'=AZUL // 'Proceso'=AMARILLO // 'Finalizado'= VERDE   // 'Vencido'=ROJO ,  )

    //RELACIONAR USUARIO TAREA
    $scope.peopleHeader = [
        "Nombre",
        "Id usuario",
    ];
    $scope.registros = [];
    $scope.DialogoEliminar = function (ID) {
        alertaEliminar(ID);
    };
    function alertaEliminar(id) {
        quitarlinea();
        //$scope.$apply()
    }
    function quitarlinea(ID) {
        var index = -1;
        var comArr = eval($scope.registros);
        for (var i = 0; i < comArr.length; i++) {
            if (comArr[i].id === ID) {
                index = i;
            }
        }
        $scope.registros.splice(index, 1)
    }
    $scope.add = function (inputEncargado) {
        var newId = $scope.registros.length;
        newId++;
        $scope.registros.push({
            //id: newId,
            nombre: $scope.inputEncargado.nombre,
            ID: $scope.inputEncargado.id_usuario,
        });

        $scope.regis = $scope.registros;
    }

    $scope.crearTarea = function () {
        $ngConfirm({
            title: 'Información',
            content: "'La tarea han sido creada con exito '" + "' Este mensaje se cerrara en 2 segundos.'",
            autoClose: 'logoutUser|2000',
            buttons: {
                logoutUser: {
                    text: 'ACEPTAR',
                    btnClass: 'btn-green',
                    action: function () {
                        $scope.crearTareas();
                    }
                },

            }
        });
    }
    $scope.crearTareas = function () {

        var CreaTareas = {
            nombre: $scope.inputNombre,
            listUsers: $scope.regis,
            fechaDesde: $scope.fechaDesde,
            fechaHasta: $scope.fechaHasta,
            FECHACREACION: $scope.fechahoy,
            descripcion: $scope.inputFuncion,
            ESTADO: $scope.estado = "Ingresado",
            ESTADO2: $scope.estado2 = "Activo"

        };
        $http({
            method: 'POST',
            data: CreaTareas,
            url: "https://localhost:44323//Tareas/CrearTareas"
        }).then(
            function (response) {
                $window.location.href = "#!RegistroTareas";

                console.log("OK");
            },
            function (error) {
                console.log("ERROR ");
                console.log(error);
            });
        $window.location.href = "#!RegistroTareas";
    };
    //FIN CREACION DE TAREAS 


    //EDITAR TAREA
    $scope.vali = function () {
        var logs = [];
        angular.forEach($scope.LstTareaUser, function (uu) {
            if (!!uu.selected) {
                this.push(uu.IDUSER);
            }
        }, logs);
        $scope.cotSelected1 = logs.length;
    };
    $scope.editarTarea = function (uu, regg) {
        var log = [];
        $scope.registros111 = [];
        angular.forEach($scope.LstTareaUser, function (uu) {
            if (!!uu.selected) {
                this.push(uu.IDUSER);
                $scope.registros111.push({
                    ID: uu.IDUSER,
                });
            }
        }, log);
        console.log("idUse:" + $scope.registros111);

        if (regg.inputEstado == "Activo") {
            $scope.estadoo = "Ingresado";
        }
        if (regg.inputEstado == "Inactivo") {
            $scope.estadoo = "Vencido";
        }
        var EditarTareas = {
            ID_TAREAS: regg.id_tareas,
            listUserRem: $scope.registros111,
            listUsers: $scope.regis2,
            fechadesde: regg.fechaDesde,
            fechahasta: regg.fechaHasta,
            nombre: regg.nombre,
            descripcion: regg.descripcion,
            observaciones: regg.observaciones,
            ESTADO2: regg.inputEstado,
            ESTADO: $scope.estadoo
        };
        $http({
            method: 'POST',
            data: EditarTareas,
            url: "https://localhost:44323//Tareas/editarTareas"
        }).then(
            function (response) {
                $window.location.href = "#!RegistroTareas";

                console.log("OK");
            },
            function (error) {
                console.log("ERROR ");
                console.log(error);
            });
        $window.location.href = "#!RegistroTareas";



    };

    //  RELACIONAR AL EDITAR TAREAS
    $scope.peopleHeader2 = [
        "Nombre",
        "Id usuario",
    ];
    $scope.registros2 = [];
    $scope.DialogoEliminar2 = function (ID) {
        alertaEliminar2(ID);
    };
    function alertaEliminar2(id) {
        quitarlinea2();
        //$scope.$apply()
    }
    function quitarlinea2(ID) {
        var index2 = -1;
        var comArr2 = eval($scope.registros2);
        for (var i = 0; i < comArr2.length; i++) {
            if (comArr2[i].id === ID) {
                index2 = i;
            }
        }
        $scope.registros2.splice(index2, 1)
    }
    $scope.add2 = function (inputEncargado2) {
        var newId = $scope.registros2.length;
        newId++;
        $scope.registros2.push({
            //id: newId,
            nombre: inputEncargado2.nombre,
            ID: inputEncargado2.id_usuario,
        });

        $scope.regis2 = $scope.registros2;
    }

    //CREAR PROBLEMA

    $scope.ID = function (regg) {
        $scope.idTareas = regg.id_tareas;

    };
    $scope.crearProblema = function (regg) {

        $scope.propietario = $scope.Users.Nombre + " " + $scope.Users.apellido;

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
    $scope.verProblema = function (regg) {
        $scope.idper = regg.id_tareas

        $http({
            method: 'GET',
            url: "https://localhost:44323//Problema/ObtenerProblemaPorTarea?pageindex=" + $scope.indiceTareas + "&pagesize=" + $scope.itemsTareas + "&ID_TAREA=" + $scope.idper
        }).then(
            function success(response) {
                $scope.listProblemas = response.data.Resultado;
                $scope.totalItems1 = response.data.Total;

                $scope.respuestUser = response.data.Resultado;
                if ($scope.respuestUser.length <= 0) {
                    $ngConfirm({
                        title: 'Información',
                        content: "No fue posible obtener el problema asociado o no existe registro",
                        /*autoClose: 'logoutUser|3000',*/
                        buttons: {
                            logoutUser: {
                                text: 'ACEPTAR',
                                btnClass: 'btn-green',
                                action: function () {
                                    $scope.CerrarModal('#modalMiProblema');
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
    //FIN VISTA problema
    //FIN CREAR PROBLEMA


});