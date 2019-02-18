var app = angular.module("myApp", ['ui.bootstrap']);
app.controller('myCtrl', function ($scope, $http, $modal) {
    $scope.judge = false;
    
    $scope.submit = function () {
        
        var u = $scope.username;
        var p = $scope.password;
        if (u == undefined) {
            alert("用户名不能为空！");
        }
        else if (p == undefined) {
            alert("密码不能为空！");
        }
        else {
            document.getElementById("login").innerText = "正在登录请稍后...";
            $scope.judge = true;
            $http({
                method: 'GET',
                url: 'http://localhost:5451/TrainApp/UserLogin',
                params: {
                    'username': u,
                    'password': p
                }
            }).then(function successCallback(response) {
                $scope.objectId = response.data;
                if ($scope.objectId === "登录失败") {
                    document.getElementById("login").innerText = "登录";
                    $scope.judge = false;
                    alert("登录失败，请重新登录！");
                }
                else {
                    document.getElementById("login").innerText = "登录";
                    $scope.judge = false;
                    window.location = "View/courseList.html";
                }
            }, function errorCallback(response) {
            });

        }
    }

    $scope.openReset = function () {
        var data = "通过modal传递的数据";
        var modalInstance = $modal.open({
            templateUrl: 'resetPassword.html',//script标签中定义的id
            controller : 'modalCtrl',//modal对应的Controller
            resolve : {
                data : function() {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
    }

})

app.controller('modalCtrl', function ($scope, $http, $modalInstance, data) {

    $scope.closeDialog = function () {
        $modalInstance.dismiss('cancel');
    }
    
    $scope.sendEmail = function () {
        var email = $scope.myEmail;
        if (email == undefined) {
            alert("用户名不能为空！");
        } else {
            document.getElementById("title").innerText = "提示";
            document.getElementById("message").innerText = "正在发送邮件请稍后...";
            $http({
                method: 'GET',
                url: 'http://localhost:5451/TrainApp/ResetPassword',
                params: {
                    'email': email
                }
            }).then(function successCallback(response) {
                $modalInstance.dismiss('cancel');
                alert("邮件发送成功，请前往邮箱重置密码。");                
            }, function errorCallback(response) {
                $modalInstance.dismiss('cancel');
                alert("邮件发送失败，请重试。");
            });
        }
    }
    

})