angular.module('app').controller('logincontroller', function ($scope, $http, $location, $ngConfirm, $window, $rootScope, $localStorage, $sessionStorage) {
    /*sessionStorage.clear();*/
    $sessionStorage.$reset();

    $scope.Message = "Bienvenido!";
    $scope.spinner = false;
    $scope.recu = false;
    $scope.log = true;
    $scope.login = [];

    $scope.volveLog = function () {
        $scope.recu = false;
        $scope.log = true;
    }
    //LOGIN 
    $scope.ValidarLogin = function () {
        var Usuario = $scope.user;
        var Password = $scope.pass;

        $scope.spinner = true;
        $scope.recu = false;
        $scope.log = false;

        $http({
            method: "Get",
            url: "https://localhost:44323//Login/ValidaLogin?usuario=" + Usuario + "&contrasenna=" + Password
        }).then(

            function success(response) {
                if (response.data) {
                    /*$scope.User = response.data;*/
                var reg = response.data;
                 sessionStorage.setItem("user", JSON.stringify(reg));

                    console.log("OK");
                    $window.location = "../MenuPrincipal/";
                }

                else {
                    $scope.cargando = false;
                    $scope.loginForm = true;
                    $ngConfirm({
                        title: 'Información',
                        content: "Usuario o contraseña invalida",
                        scope: $scope,
                        buttons: {
                            Aceptar: {
                                text: 'Aceptar',
                                action: function (scope, button) {

                                    $window.location = "../InicioSesion/";

                                }
                            }

                        }
                    });
                }

            },

            function error(response) {
                console.log(response);
            });
    };


    $scope.ValidarLogin1 = function () {
        console.log("paso por aqui");

        if ($scope.user == "seba" && $scope.pass == "1234") {
            $ngConfirm({
                boxWidth: '320px',
                useBootstrap: false,
                animation: 'zoom',
                closeAnimation: 'scaleY',
                icon: 'fas fa-info-circle',
                title: 'Información',
                type: 'green',
                theme: 'modern',
                content: "<strong> Usuario si existe  </strong>",
                scope: $scope,
                buttons: {
                    Si: {
                        btnClass: 'btn-blue',
                        text: 'Aceptar',
                        action: function (scope, button) {
                            $window.location = "../MenuPrincipal/";
                        }
                    }
                }
            });

        }
        else {

            $ngConfirm({
                boxWidth: '320px',
                useBootstrap: false,
                animation: 'zoom',
                closeAnimation: 'scaleY',
                icon: 'fas fa-info-circle',
                title: 'Información',
                type: 'blue',
                theme: 'modern',
                content: "<strong> Usuario Deshabilitado o no existe  </strong>" ,
                scope: $scope,
                buttons: {
                    Si: {
                        btnClass: 'btn-blue',
                        text: 'Aceptar',
                        action: function (scope, button) {
                        }
                    }
                }
            });

        }

    };

    //recuperar contraseña
    $scope.ValidarRecuperacion = function () {
        var recuperaemail = {
            email1: $scope.inputemail,
        };
        $http({
            method: 'POST',
            data: recuperaemail,
            url: "https://localhost:44323/Login/RecuperarContrasenia"
        }).then(
            function (response) {
                $ngConfirm({
                    title: 'Información',
                    content: "'Su contraseña ha sido enviada al correo ingresado'" + "' Este mensaje se cerrara en 3 segundos.'",
                    autoClose: 'logoutUser|3000',
                    buttons: {
                        logoutUser: {
                            text: 'ACEPTAR',
                            btnClass: 'btn-green',
                            action: function () {
                                $window.location = "../InicioSesion/";
                            }
                        },
                    }
                });
                console.log("OK");
            },
            function (error) {
                console.log("ERROR ");
                console.log(error);
            });
        $scope.inputemail = null;
    };

    $scope.recuperar = function () {
        $scope.recu = true;
        $scope.log = false;
        //$window.location = "#!/Recuperar";

    };




});