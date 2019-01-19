var app = angular.module("myApp", ['ui.bootstrap']);
Bmob.initialize("f69acbf2dd96fbaefdf9fd9793e93f66", "9a956445ff932b7d3f59b81af28cbe2a")

app.controller('myCtrl', function ($scope, $http) {
    $scope.a = 0
})

app.controller('classCtrl', function ($rootScope, $scope, $http, $modal) {

    var classesId = [];

    $scope.showAllClass = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowClass',
        }).then(function successCallback(response) {
            $rootScope.classes = response.data;
        }, function errorCallback(response) {
            alert("获取数据失败");
        });
    }

    $scope.showAllClass();
    $scope.shareObject = function (obj) {
        obj = obj || {};
        //将事件以"ShareObjectEvent"为名进行广播
        $rootScope.$broadcast("ShareClassesEvent", obj);
    };

    $scope.allChecked = false;

    $scope.chooseAll = function () {
        $scope.allChecked = event.target.checked;
        for (i in $scope.classes) {
            $scope.classes[i].checked = event.target.checked;
        }
    };

    $scope.chooseClass = function (index) {
        for (i in $scope.classes) {
            if (!$scope.classes[i].checked) {
                $scope.allChecked = false;
                return;
            }
        }
        $scope.allChecked = true;
    }

    $scope.openAddClass = function () {
        var data = "通过modal传递的数据";
        var modalInstance = $modal.open({
            templateUrl: 'addClasses.html',//script标签中定义的id
            controller: 'addClassCtrl',//modal对应的Controller
            resolve: {
                data: function () {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
        modalInstance.result.then(function (result) {
            if (result == "success") {
                $scope.showAllClass();
            }
        })
    }

    var count = 0;
    var a = 0;

    $scope.deleteClass = function () {
        if ($scope.allChecked == true) {
            if (confirm("班级删除后，学生信息以及答题情况也将被全部清除。确认删除所有班级？")) {
                for (i in $scope.classes) {
                    a++;
                    $scope.delete($scope.classes[i].id);
                }
            }
        } else {
            for (i in $scope.classes) {
                if ($scope.classes[i].checked) {
                    a++;
                }
            }
            if (a == 0) {
                alert("请先选择想要删除的班级。");
                return;
            }
            if (confirm("班级删除后，学生信息以及答题情况也将被全部清除。确认删除所选班级？")) {
                a = 0;
                for (i in $scope.classes) {
                    if ($scope.classes[i].checked) {
                        a++;
                        $scope.delete($scope.classes[i].id);
                    }
                }
            } 
        }
    }

    $scope.delete = function (id) {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/DeleteClass',
            params: {
                'id': id
            }
        }).then(function successCallback(response) {
            count++;
            if (count == a) {
                $scope.showAllClass();
                alert("删除成功");
            }
        }, function errorCallback(response) {
            alert("班号为" + id + "的班级删除失败");
            count++;
            if (count == a) {
                $scope.showAllClass();
            }
        });
    }

    $scope.modifyClassInfo = function () {
        var data = [];
        for (i in $scope.classes) {
            if ($scope.classes[i].checked) {
                data.push($scope.classes[i]);
            }
        }
        if (data.length == 0) {
            alert("请先选择想要修改信息的班级。");
            return;
        }
        var modalInstance = $modal.open({
            templateUrl: 'updateClasses.html',//script标签中定义的id
            controller: 'updateClassCtrl',//modal对应的Controller
            resolve: {
                data: function () {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
        modalInstance.result.then(function (result) {
            if (result == "success") {
                $scope.showAllClass();
            }
        })
    }

})

app.controller('addClassCtrl', function ($scope, $http, $modalInstance, data) {

    $scope.closeDialog = function () {
        $modalInstance.dismiss('cancel');
    }

    $scope.submitClassInfo = function () {
        var id = $scope.classId;
        var name = $scope.className;
        var schedule = $scope.classSchedule;
        if (id == undefined) {
            alert("请输入班级号，该项为必填项！");
        } else if (name == undefined) {
            alert("请输入班级名称，该项为必填项！");
        } else {
            $http({
                method: 'POST',
                url: 'http://localhost:5451/TrainApp/AddClass',
                data: { "id": id, "name": name, "schedule": schedule }
            }).then(function successCallback(response) {
                $scope.result = response.data;
                $modalInstance.close("success");
                alert("添加班级成功。");
            }, function errorCallback(response) {
                $modalInstance.close(response.data);
                alert("添加班级失败。");
            });
        }
    }


})

app.controller('updateClassCtrl', function ($scope, $http, $modalInstance, data) {

    $scope.classList = data;
    $scope.closeDialog = function () {
        $modalInstance.dismiss('cancel');
    }

    $scope.submitClassInfo = function () {
        for (i in $scope.classList) {
            if ($scope.classList[i].name == "") {
                alert("班号为：" + $scope.classList[i].id + "的班级名称不能为空哦！");
                return;
            } else {
                continue;
            }
        }
        $scope.update();
    }

    $scope.update = function () {
        $http({
            method: 'POST',
            url: 'http://localhost:5451/TrainApp/UpdateClass',
            data:JSON.stringify($scope.classList)
        }).then(function successCallback(response) {
            alert("所选班级的信息修改成功！");
            $modalInstance.close("success");
        }, function errorCallback(response) {
            alert("所选班级的信息修改失败！");
        });
    }


})

app.controller('studentCtrl', function ($scope, $http, $modal) {

    app.directive('file', function () {
        return {
            scope: {
                file: '='
            },
            link: function (scope, el, attrs) {
                el.bind('change', function (event) {
                    var file = event.target.files;
                    scope.file = file ? file : undefined;
                    scope.$apply();
                });
            }
        };
    })

    $scope.fileChanged = function (ele) {
        $scope.files = ele.files;
        var file = document.querySelector('input[type=file]').files[0];
        var filename = $scope.files[0].name;
        if (filename.length > 1 && filename) {
            var ldot = filename.lastIndexOf(".");
            var type = filename.substring(ldot + 1);  //文件类型
            $scope.fileName = filename.slice(0, ldot);//文件名    
            //读取文件内容  
            var reader = new FileReader();
            reader.onload = function (ev) {
                try {
                    var data = ev.target.result,
                        workbook = XLSX.read(data, {
                            type: 'binary'
                        }), // 以二进制流方式读取得到整份excel表格对象
                        persons = []; // 存储获取到的数据
                } catch (e) {
                    alert('文件类型不正确,请选择xlsx格式的文件！');
                    return;
                }
                // 表格的表格范围，可用于判断表头是否数量是否正确
                var fromTo = '';
                // 遍历每张表读取
                for (var sheet in workbook.Sheets) {
                    if (workbook.Sheets.hasOwnProperty(sheet)) {
                        fromTo = workbook.Sheets[sheet]['!ref'];
                        console.log(fromTo);
                        persons = persons.concat(XLSX.utils.sheet_to_json(workbook.Sheets[sheet]));
                        // break; // 如果只取第一张表，就取消注释这行
                    }
                }
                $scope.p = persons;
                $scope.$apply(); //传播Model的变化。//很重要，等文件内容读出完以后，开启脏检测,这样就将文件名字，内容绑定到界面上显示
            }
            reader.readAsBinaryString(file);
        }
    }

    $scope.addStudent = function () {
        $http({
            method: 'POST',
            url: 'http://localhost:5451/TrainApp/UploadStudent',
            data: JSON.stringify($scope.students)
        }).then(function successCallback(response) {
            if (response.data == "上传成功") {
                alert("数据上传成功");
            } else {
                alert("部分数据上传失败，请重试");
            }
        }, function errorCallback(response) {
            alert("数据上传失败，请重试");
        });
    }

    $scope.$on("ShareClassesEvent", function (event, args) {
        classes = args;
    });

    $scope.showAllStudents = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/showAllStudent',
        }).then(function successCallback(response) {
            $scope.students = response.data;
            $scope.pageSize = 10;
            $scope.pages = Math.ceil($scope.students.length / $scope.pageSize); //分页数
            $scope.newPages = $scope.pages > 5 ? 5 : $scope.pages;
            $scope.pageList = [];
            $scope.selPage = 1;
            $scope.items = $scope.students.slice(0, $scope.pageSize);
            //分页要repeat的数组
            for (var i = 0; i < $scope.newPages; i++) {
                $scope.pageList.push(i + 1);
            }
        }, function errorCallback(response) {
            alert("获取数据失败");
        });
        
    }

    $scope.showAllStudents();

    $scope.showStudentByClass = function (classId) {
        if (classId == null) {
            $scope.showAllStudents();
        } else {
            $http({
                method: 'GET',
                url: 'http://localhost:5451/TrainApp/showStudentById',
                params: {
                    'classId': classId
                }
            }).then(function successCallback(response) {
                $scope.students = response.data;
                $scope.pageSize = 10;
                $scope.pages = Math.ceil($scope.students.length / $scope.pageSize); //分页数
                $scope.newPages = $scope.pages > 5 ? 5 : $scope.pages;
                $scope.pageList = [];
                $scope.selPage = 1;
                $scope.items = $scope.students.slice(0, $scope.pageSize);
                //分页要repeat的数组
                for (var i = 0; i < $scope.newPages; i++) {
                    $scope.pageList.push(i + 1);
                }
            }, function errorCallback(response) {
                alert("获取数据失败");
            });
        }
    }

    $scope.allChecked = false;

    $scope.chooseAll = function () {
        $scope.allChecked = event.target.checked;
        for (i in $scope.students) {
            $scope.students[i].checked = event.target.checked;
        }
    };

    $scope.chooseClass = function (index) {
        for (i in $scope.students) {
            if (!$scope.students[i].checked) {
                $scope.allChecked = false;
                return;
            }
        }
        $scope.allChecked = true;
    }

    $scope.openAddStudent = function () {
        var data = "通过modal传递的数据";
        var modalInstance = $modal.open({
            templateUrl: 'addStudent.html',//script标签中定义的id
            controller: 'addStudentCtrl',//modal对应的Controller
            resolve: {
                data: function () {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
        modalInstance.result.then(function (result) {
            if (result == "success") {
                $scope.showAllStudents();
            }
        })
    }

    //设置表格数据源(分页)
    $scope.setData = function () {
        $scope.items = $scope.students.slice(($scope.pageSize * ($scope.selPage - 1)), ($scope.selPage * $scope.pageSize));//通过当前页数筛选出表格当前显示数据
    }
    
    //打印当前选中页索引
    $scope.selectPage = function (page) {
        //不能小于1大于最大
        if (page < 1 || page > $scope.pages) return;
        //最多显示分页数5
        if (page > 2) {
            //因为只显示5个页数，大于2页开始分页转换
            var newpageList = [];
            for (var i = (page - 3) ; i < ((page + 2) > $scope.pages ? $scope.pages : (page + 2)) ; i++) {
                newpageList.push(i + 1);
            }
            $scope.pageList = newpageList;
        }
        $scope.selPage = page;
        $scope.setData();
        $scope.isActivePage(page);
    };
    //设置当前选中页样式
    $scope.isActivePage = function (page) {
        return $scope.selPage == page;
    };
    //上一页
    $scope.Previous = function () {
        $scope.selectPage($scope.selPage - 1);
    }
    //下一页
    $scope.Next = function () {
        $scope.selectPage($scope.selPage + 1);
    };

    var count = 0;
    var a = 0;

    $scope.deleteStudent = function () {
        if ($scope.allChecked == true) {
            if (confirm("删除？")) {
                for (i in $scope.items) {
                    a++;
                    $scope.delete($scope.items[i].objectId);
                }
            }
        } else {
            for (i in $scope.items) {
                if ($scope.items[i].checked) {
                    a++;
                }
            }
            if (a == 0) {
                alert("请先选择想要删除的学生。");
                return;
            }
            if (confirm("删除？")) {
                for (i in $scope.items) {
                    if ($scope.items[i].checked) {
                        a++;
                        $scope.delete($scope.items[i].objectId);
                    }
                }
            }
        }
    }

    $scope.delete = function (id) {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/DeleteStudent',
            params: {
                'objectId': id
            }
        }).then(function successCallback(response) {
            count++;
            if (count == a) {
                $scope.showAllStudents();
                alert("删除成功");
            }
        }, function errorCallback(response) {
            alert("学号为" + id + "的学生删除失败");
            count++;
            if (count == a) {
                $scope.showAllStudents();
            }
        });
    }

})

app.controller('addStudentCtrl', function ($scope, $http, $modalInstance, data) {

    $scope.closeDialog = function () {
        $modalInstance.dismiss('cancel');
    }

    $scope.submitClassInfo = function () {
        var username = $scope.username;
        var name = $scope.name;
        var password = $scope.password;
        var pwd = $scope.passwordAgain;
        var identity = "student";
        var firstTime = 0;
        if (username == undefined) {
            alert("请输入学号，该项为必填项！");
        } else if (name == undefined) {
            alert("请输入学生姓名，该项为必填项！");
        } else if (password == undefined || pwd == undefined) {
            alert("请输入初始密码，该项为必填项！");
        } else if(password != pwd){
            alert("两次输入的密码需保持一致，请确认密码！");
        } else {
            $http({
                method: 'POST',
                url: 'http://localhost:5451/TrainApp/AddStudent',
                data: { "username": username, "name": name, "password": password }
            }).then(function successCallback(response) {
                $scope.result = response.data;
                $modalInstance.close("success");
                alert("添加学生成功。");
            }, function errorCallback(response) {
                $modalInstance.close(response.data);
                alert("添加学生失败。");
            });
        }
    }


})

app.controller('courseCtrl', function ($scope, $http, $modal) {

    $scope.changeCourse = function (courseId, courseName) {
        $scope.currentCourse = courseId;
        $scope.courseName = courseName;
    }
    
    $scope.showAllCourse = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowCourse',
        }).then(function successCallback(response) {
            $scope.courses = response.data;
        }, function errorCallback(response) {
            alert("获取数据失败");
        });
    }
    $scope.showAllCourse();

    $scope.showFileContent = function (index, type) {
        var url = "";
        if (type == 1) {
            url = $scope.courses[index].programu;
        } else if (type == 2) {
            url = $scope.courses[index].experimentu;
        } else {
            url = $scope.courses[index].timeu;
        }
        if (url == "") {
            alert("暂未提供该文件，请上传！");
        } else {
            window.open(url, "_blank");
        }
    }

    $scope.allChecked = false;

    $scope.chooseAll = function () {
        $scope.allChecked = event.target.checked;
        for (i in $scope.courses) {
            $scope.courses[i].checked = event.target.checked;
        }
    };

    $scope.chooseCourse = function (index) {
        for (i in $scope.courses) {
            if (!$scope.courses[i].checked) {
                $scope.allChecked = false;
                return;
            }
        }
        $scope.allChecked = true;
    }

    $scope.openAddCourse = function () {
        var data = "通过modal传递的数据";
        var modalInstance = $modal.open({
            templateUrl: 'addCourses.html',//script标签中定义的id
            controller: 'addCourseCtrl',//modal对应的Controller
            resolve: {
                data: function () {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
        modalInstance.result.then(function (result) {
            if (result == "success") {
                $scope.showAllCourse();
            }
        })
    }

    var count = 0;
    var a = 0;

    $scope.deleteClass = function () {
        if ($scope.allChecked == true) {
            if (confirm("班级删除后，学生信息以及答题情况也将被全部清除。确认删除所有班级？")) {
                for (i in $scope.classes) {
                    a++;
                    $scope.delete($scope.classes[i].id);
                }
            }
        } else {
            if (confirm("班级删除后，学生信息以及答题情况也将被全部清除。确认删除所选班级？")) {
                for (i in $scope.classes) {
                    if ($scope.classes[i].checked) {
                        a++;
                        $scope.delete($scope.classes[i].id);
                    }
                }
            }
        }
    }

    $scope.delete = function (id) {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/DeleteClass',
            params: {
                'id': id
            }
        }).then(function successCallback(response) {
            count++;
            if (count == a) {
                $scope.showAllClass();
                alert("删除成功");
            }
        }, function errorCallback(response) {
            alert("班号为" + id + "的班级删除失败");
            count++;
            if (count == a) {
                $scope.showAllClass();
            }
        });
    }

    $scope.modifyClassInfo = function () {
        var data = [];
        for (i in $scope.classes) {
            if ($scope.classes[i].checked) {
                data.push($scope.classes[i]);
            }
        }
        var modalInstance = $modal.open({
            templateUrl: 'updateClasses.html',//script标签中定义的id
            controller: 'updateClassCtrl',//modal对应的Controller
            resolve: {
                data: function () {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
        modalInstance.result.then(function (result) {
            if (result == "success") {
                $scope.showAllClass();
            }
        })
    }

})

app.controller('queryQuestionCtrl', function ($scope, $http, $modal) {

    $scope.showAllQuestion = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowQuestion',
        }).then(function successCallback(response) {
            $scope.questions = response.data;
            $scope.pageSize = 10;
            $scope.pages = Math.ceil($scope.questions.length / $scope.pageSize); //分页数
            $scope.newPages = $scope.pages > 5 ? 5 : $scope.pages;
            $scope.pageList = [];
            $scope.selPage = 1;
            $scope.items = $scope.questions.slice(0, $scope.pageSize);
            //分页要repeat的数组
            for (var i = 0; i < $scope.newPages; i++) {
                $scope.pageList.push(i + 1);
            }

        }, function errorCallback(response) {
            alert("获取数据失败");
        });
    }

    $scope.showAllQuestion();
    $scope.allChecked = false;

    $scope.chooseAll = function () {
        $scope.allChecked = event.target.checked;
        for (i in $scope.items) {
            $scope.items[i].checked = event.target.checked;
        }
    };

    $scope.chooseQuestion = function (index) {
        for (i in $scope.items) {
            if (!$scope.items[i].checked) {
                $scope.allChecked = false;
                return;
            }
        }
        $scope.allChecked = true;
    }

    $scope.openAddQuestion = function () {
        var data = "通过modal传递的数据";
        var modalInstance = $modal.open({
            templateUrl: 'addQuestions.html',//script标签中定义的id
            controller: 'addQuestionCtrl',//modal对应的Controller
            resolve: {
                data: function () {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
        modalInstance.result.then(function (result) {
            if (result == "success") {
                $scope.showAllQuestion();
            }
        })
    }

    var count = 0;
    var a = 0;

    $scope.deleteQuestion = function () {
        if ($scope.allChecked == true) {
            if (confirm("删除所有题目？")) {
                for (i in $scope.items) {
                    a++;
                    $scope.delete($scope.items[i].id, $scope.items[i].courseId);
                }
            }
        } else {
            if (confirm("确认删除所选题目？")) {
                for (i in $scope.items) {
                    if ($scope.items[i].checked) {
                        a++;
                        $scope.delete($scope.items[i].id, $scope.items[i].courseId);
                    }
                }
            }
        }
    }

    $scope.delete = function (id, courseId) {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/DeleteQuestion',
            params: {
                'id': id,
                'courseId': courseId
            }
        }).then(function successCallback(response) {
            count++;
            if (count == a) {
                $scope.showAllQuestion();
                alert("删除成功");
            }
        }, function errorCallback(response) {
            alert("题号为" + id + "的题目删除失败");
            count++;
            if (count == a) {
                $scope.showAllQuestion();
            }
        });
    }

    $scope.modifyQuestionInfo = function () {
        var data = [];
        for (i in $scope.items) {
            if ($scope.items[i].checked) {
                data.push($scope.items[i]);
            }
        }
        var modalInstance = $modal.open({
            templateUrl: 'updateQuestions.html',//script标签中定义的id
            controller: 'updateQuestionCtrl',//modal对应的Controller
            resolve: {
                data: function () {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
        modalInstance.result.then(function (result) {
            if (result == "success") {
                $scope.showAllQuestion();
            }
        })
    }
    //设置表格数据源(分页)
    $scope.setData = function () {
        $scope.items = $scope.questions.slice(($scope.pageSize * ($scope.selPage - 1)), ($scope.selPage * $scope.pageSize));//通过当前页数筛选出表格当前显示数据
    }

    //打印当前选中页索引
    $scope.selectPage = function (page) {
        //不能小于1大于最大
        if (page < 1 || page > $scope.pages) return;
        //最多显示分页数5
        if (page > 2) {
            //因为只显示5个页数，大于2页开始分页转换
            var newpageList = [];
            for (var i = (page - 3) ; i < ((page + 2) > $scope.pages ? $scope.pages : (page + 2)) ; i++) {
                newpageList.push(i + 1);
            }
            $scope.pageList = newpageList;
        }
        $scope.selPage = page;
        $scope.setData();
        $scope.isActivePage(page);
    };
    //设置当前选中页样式
    $scope.isActivePage = function (page) {
        return $scope.selPage == page;
    };
    //上一页
    $scope.Previous = function () {
        $scope.selectPage($scope.selPage - 1);
    }
    //下一页
    $scope.Next = function () {
        $scope.selectPage($scope.selPage + 1);
    };

    //下拉框
    $scope.showCourse = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowCourse'
        }).then(function successCallback(response) {
            $scope.courseInfo = response.data;
        }, function errorCallback(response) {
        });
    }
    $scope.showCourse();
    $scope.selectedUnit = function (courseId) {
        $scope.unitId = "";
        if (courseId != "") {
            $http({
                method: 'GET',
                url: 'http://localhost:5451/TrainApp/SelectedUnit',
                params: {
                    'courseId': courseId
                }
            }).then(function successCallback(response) {
                $scope.unitInfo = response.data;
            }, function errorCallback(response) {
                alert("查询失败");
            });
        }
        else { alert("错误"); }
    }


    $scope.queryQuestion = function (courseId, unitId) {
        //console.log(courseId + " " + unitId);
        if (courseId != "" && courseId != null && courseId != undefined) {
            if (unitId == "" || unitId == null || unitId == undefined) {
                $http({
                    method: 'GET',
                    url: 'http://localhost:5451/TrainApp/QueryCourseQuestion',
                    params: {
                        'courseId': courseId
                    }
                }).then(function successCallback(response) {
                    $scope.questionInfo = response.data;
                    alert("查询成功");
                    $scope.pageSize = 10;
                    $scope.pages = Math.ceil($scope.questionInfo.length / $scope.pageSize); //分页数
                    $scope.newPages = $scope.pages > 5 ? 5 : $scope.pages;
                    $scope.pageList = [];
                    $scope.selPage = 1;
                    $scope.items = $scope.questionInfo.slice(0, $scope.pageSize);
                    //分页要repeat的数组
                    for (var i = 0; i < $scope.newPages; i++) {
                        $scope.pageList.push(i + 1);
                    }
                }, function errorCallback(response) {
                    alert("查询失败");
                });
            }
            else {
                $http({
                    method: 'GET',
                    url: 'http://localhost:5451/TrainApp/QueryUnitQuestion',
                    params: {
                        'courseId': courseId,
                        'unitId': unitId
                    }
                }).then(function successCallback(response) {
                    $scope.questionInfo = response.data;
                    alert("查询成功");
                    $scope.pageSize = 10;
                    $scope.pages = Math.ceil($scope.questionInfo.length / $scope.pageSize); //分页数
                    $scope.newPages = $scope.pages > 5 ? 5 : $scope.pages;
                    $scope.pageList = [];
                    $scope.selPage = 1;
                    $scope.items = $scope.questionInfo.slice(0, $scope.pageSize);
                    //分页要repeat的数组
                    for (var i = 0; i < $scope.newPages; i++) {
                        $scope.pageList.push(i + 1);
                    }
                }, function errorCallback(response) {
                    alert("查询失败");
                });
            }
        }
        else {
            alert("请选择要查询的课程名");
        }
    }

})

app.controller('questionCtrl', function ($scope, $http, $modal) {

    var classesId = [];

    $scope.showAllQuestion = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowQuestion',
        }).then(function successCallback(response) {
            $scope.questions = response.data;
            $scope.pageSize = 10;
            $scope.pages = Math.ceil($scope.questions.length / $scope.pageSize); //分页数
            $scope.newPages = $scope.pages > 5 ? 5 : $scope.pages;
            $scope.pageList = [];
            $scope.selPage = 1;
            $scope.items = $scope.questions.slice(0, $scope.pageSize);
            //分页要repeat的数组
            for (var i = 0; i < $scope.newPages; i++) {
                $scope.pageList.push(i + 1);
            }

        }, function errorCallback(response) {
            alert("获取数据失败");
        });
    }

    $scope.showAllQuestion();
    app.directive('file', function () {
        return {
            scope: {
                file: '='
            },
            link: function (scope, el, attrs) {
                el.bind('change', function (event) {
                    var file = event.target.files;
                    scope.file = file ? file : undefined;
                    scope.$apply();
                });
            }
        };
    })

    $scope.fileChanged = function (ele) {

        $scope.files = ele.files;
        var file = document.querySelector('input[type=file]').files[0];

        var filename = $scope.files[0].name;

        if (filename.length > 1 && filename) {

            var ldot = filename.lastIndexOf(".");

            var type = filename.substring(ldot + 1);  //文件类型

            $scope.fileName = filename.slice(0, ldot);//文件名    

            //读取文件内容  
            var reader = new FileReader();
            reader.onload = function (ev) {
                try {
                    var data = ev.target.result,
                        workbook = XLSX.read(data, {
                            type: 'binary'
                        }), // 以二进制流方式读取得到整份excel表格对象
                        persons = []; // 存储获取到的数据
                } catch (e) {
                    console.log('文件类型不正确，请选择.xlsx格式的文件');
                    return;
                }
                // 表格的表格范围，可用于判断表头是否数量是否正确
                var fromTo = '';
                // 遍历每张表读取
                for (var sheet in workbook.Sheets) {
                    if (workbook.Sheets.hasOwnProperty(sheet)) {
                        fromTo = workbook.Sheets[sheet]['!ref'];
                        console.log(fromTo);
                        persons = persons.concat(XLSX.utils.sheet_to_json(workbook.Sheets[sheet]));
                        // break; // 如果只取第一张表，就取消注释这行
                    }
                }
                console.log(persons);
                $scope.p = persons;
                $http({
                    method: 'POST',
                    contentType: 'application/json',
                    url: 'http://localhost:5451/TrainApp/GetQuestion',
                    data: JSON.stringify(persons)
                }).then(function successCallback(response) {
                    $scope.question = response.data;
                }, function errorCallback(response) {
                });
                $scope.$apply(); //传播Model的变化。//很重要，等文件内容读出完以后，开启脏检测,这样就将文件名字，内容绑定到界面上显示
            }


            reader.readAsBinaryString(file);
        }

    }

    $scope.uploadQuestion = function () {
        //alert("正在上传···请等待");
        document.getElementById("upload").innerText = "正在上传请稍后...";
        $http({
            method: 'POST',
            url: 'http://localhost:5451/TrainApp/UploadQuestion',
            data: JSON.stringify($scope.p)
        }).then(function successCallback(response) {
            $scope.objectId = response.data;
            alert($scope.objectId);
            document.getElementById("upload").innerText = "Submit";
        }, function errorCallback(response) {

        });
    }
    $scope.showAllQuestion();

    $scope.allChecked = false;

    $scope.chooseAll = function () {
        $scope.allChecked = event.target.checked;
        for (i in $scope.items) {
            $scope.items[i].checked = event.target.checked;
        }
    };

    $scope.chooseQuestion = function (index) {
        for (i in $scope.items) {
            if (!$scope.items[i].checked) {
                $scope.allChecked = false;
                return;
            }
        }
        $scope.allChecked = true;
    }

    $scope.openAddQuestion = function () {
        var data = "通过modal传递的数据";
        var modalInstance = $modal.open({
            templateUrl: 'addQuestions.html',//script标签中定义的id
            controller: 'addQuestionCtrl',//modal对应的Controller
            resolve: {
                data: function () {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
        modalInstance.result.then(function (result) {
            if (result == "success") {
                $scope.showAllQuestion();
            }
        })
    }

    var count = 0;
    var a = 0;

    $scope.deleteQuestion = function () {
        if ($scope.allChecked == true) {
            if (confirm("删除所有题目？")) {
                for (i in $scope.items) {
                    a++;
                    $scope.delete($scope.items[i].id, $scope.items[i].courseId);
                }
            }
        } else {
            if (confirm("确认删除所选题目？")) {
                for (i in $scope.items) {
                    if ($scope.items[i].checked) {
                        a++;
                        $scope.delete($scope.items[i].id, $scope.items[i].courseId);
                    }
                }
            }
        }
    }

    $scope.delete = function (id, courseId) {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/DeleteQuestion',
            params: {
                'id': id,
                'courseId': courseId
            }
        }).then(function successCallback(response) {
            count++;
            if (count == a) {
                $scope.showAllQuestion();
                alert("删除成功");
            }
        }, function errorCallback(response) {
            alert("题号为" + id + "的题目删除失败");
            count++;
            if (count == a) {
                $scope.showAllQuestion();
            }
        });
    }

    $scope.modifyQuestionInfo = function () {
        var data = [];
        for (i in $scope.items) {
            if ($scope.items[i].checked) {
                data.push($scope.items[i]);
            }
        }
        var modalInstance = $modal.open({
            templateUrl: 'updateQuestions.html',//script标签中定义的id
            controller: 'updateQuestionCtrl',//modal对应的Controller
            resolve: {
                data: function () {//data作为modal的controller传入的参数
                    return data;//用于传递数据
                }
            }
        })
        modalInstance.result.then(function (result) {
            if (result == "success") {
                $scope.showAllQuestion();
            }
        })
    }
    //设置表格数据源(分页)
    $scope.setData = function () {
        $scope.items = $scope.questions.slice(($scope.pageSize * ($scope.selPage - 1)), ($scope.selPage * $scope.pageSize));//通过当前页数筛选出表格当前显示数据
    }

    //打印当前选中页索引
    $scope.selectPage = function (page) {
        //不能小于1大于最大
        if (page < 1 || page > $scope.pages) return;
        //最多显示分页数5
        if (page > 2) {
            //因为只显示5个页数，大于2页开始分页转换
            var newpageList = [];
            for (var i = (page - 3) ; i < ((page + 2) > $scope.pages ? $scope.pages : (page + 2)) ; i++) {
                newpageList.push(i + 1);
            }
            $scope.pageList = newpageList;
        }
        $scope.selPage = page;
        $scope.setData();
        $scope.isActivePage(page);
    };
    //设置当前选中页样式
    $scope.isActivePage = function (page) {
        return $scope.selPage == page;
    };
    //上一页
    $scope.Previous = function () {
        $scope.selectPage($scope.selPage - 1);
    }
    //下一页
    $scope.Next = function () {
        $scope.selectPage($scope.selPage + 1);
    };

})

app.controller('updateQuestionCtrl', function ($scope, $http, $modalInstance, data) {

    $scope.questionList = data;
    $scope.closeDialog = function () {
        $modalInstance.dismiss('cancel');
    }

    $scope.submitQuestionInfo = function () {
        for (i in $scope.questionList) {
            if ($scope.questionList[i].name == "") {
                alert("题目号为：" + $scope.questionList[i].id + "的题目名称不能为空哦！");
                return;
            } else {
                continue;
            }
        }
        $scope.update();
    }

    $scope.update = function () {
        $http({
            method: 'POST',
            url: 'http://localhost:5451/TrainApp/UpdateQuestion',
            data: JSON.stringify($scope.questionList)
        }).then(function successCallback(response) {
            alert("所选题目的信息修改成功！");
            $modalInstance.close("success");
        }, function errorCallback(response) {
            alert("所选题目的信息修改失败！");
        });
    }


})

app.controller('addQuestionCtrl', function ($scope, $http, $modalInstance, data) {

    $scope.closeDialog = function () {
        $modalInstance.dismiss('cancel');
    }
    $scope.submitQuestionInfo = function () {
        var question = $scope.question;
        var a = $scope.a;
        var b = $scope.b;
        var c = $scope.c;
        var d = $scope.d;
        var answer = $scope.answer;
        var analysis = $scope.analysis;
        var id = parseInt($scope.id);
        var courseId = parseInt($scope.courseId);
        var unitId = parseInt($scope.unitId);
        var knowledgeId = $scope.knowledgeId;
        var difficulty = parseInt($scope.difficulty);
        var righyNum = 0;
        var totalNum = 0;
        if (question == undefined) {
            alert("题目不能为空！");
        } else {
            //document.getElementById("title").innerText = "提示";
            //document.getElementById("submit").innerText = "正在上传请稍后...";
            $http({
                method: 'POST',
                url: 'http://localhost:5451/TrainApp/AddQuestion',
                data: { "question": question, "id": id, "a": a, "b": b, "c": c, "d": d, "answer": answer, "analysis": analysis, "courseId": courseId, "unitId": unitId, "knowledgeId": knowledgeId, "difficulty": difficulty, "righyNum": righyNum, "totalNum": totalNum }
            }).then(function successCallback(response) {
                $modalInstance.dismiss('cancel');
                $scope.showAllQuestion();
                alert("添加成功");
            }, function errorCallback(response) {
                $modalInstance.dismiss('cancel');
                alert("添加失败，请重试。");
            });
        }
    }
    //下拉框
    $scope.showCourse = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowCourse'
        }).then(function successCallback(response) {
            $scope.courseInfo = response.data;
        }, function errorCallback(response) {
        });
    }
    $scope.showCourse();

    $scope.selectedUnit = function (courseId) {
        $scope.unitId = "";
        $scope.knowledgeId = "";
        if (courseId.id != 0) {
            $http({
                method: 'GET',
                url: 'http://localhost:5451/TrainApp/SelectedUnit',
                params: {
                    'courseId': courseId
                }
            }).then(function successCallback(response) {
                $scope.unitInfo = response.data;
            }, function errorCallback(response) {
                alert("查询失败");
            });
        }
        else { alert("错误"); }
    }
    $scope.selectedKnowledge = function (unitId, courseId) {
        $scope.knowledgeId = "";
        if (unitId.id != 0) {
            $http({
                method: 'GET',
                url: 'http://localhost:5451/TrainApp/SelectedKnowledge',
                params: {
                    'unitId': unitId,
                    'courseId': courseId
                }
            }).then(function successCallback(response) {
                $scope.knowledgeInfo = response.data;
            }, function errorCallback(response) {
                alert("查询失败");
            });
        }
        else { alert("错误"); }
    }

})

app.controller('statisticCtrl', function ($scope, $http) {

    //下拉框
    $scope.showCourse = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowCourse'
        }).then(function successCallback(response) {
            $scope.courseInfo = response.data;
        }, function errorCallback(response) {
        });
    }
    $scope.showCourse();
    $scope.selectedUnit = function (courseId) {
        $scope.unitId = "";
        if (courseId != undefined) {
            $http({
                method: 'GET',
                url: 'http://localhost:5451/TrainApp/SelectedUnit',
                params: {
                    'courseId': courseId
                }
            }).then(function successCallback(response) {
                $scope.unitInfo = response.data;
            }, function errorCallback(response) {
                alert("查询失败");
            });
        }
        else { alert("错误"); }
    }


    $scope.questionStatistic = function (courseId, unitId) {
        //console.log(courseId + " " + unitId);
        if (courseId != "" && courseId != null && courseId != undefined) {
            if(unitId != "" && unitId != null && unitId != undefined){
                $http({
                    method: 'GET',
                    url: 'http://localhost:5451/TrainApp/QuestionStatistic',
                    params: {
                        'courseId': courseId,
                        'unitId': unitId
                    }
                }).then(function successCallback(response) {
                    $scope.questionInfo = response.data;
                    if ($scope.questionInfo.length > 15) {
                        $scope.highQuestionInfo = $scope.questionInfo.slice(0,15);
                        $scope.lowQuestionInfo = $scope.questionInfo.slice(-15);
                        //console.log($scope.lowQuestionInfo);
                    }
                    else {
                        $scope.highQuestionInfo = $scope.questionInfo;
                        $scope.lowQuestionInfo = $scope.questionInfo;
                    }
                    console.log($scope.questionInfo.slice(0, 1));
                    alert("查询成功");
                },function errorCallback(response) {
                    alert("查询失败");
                });
            }
            else {

                alert("请选择要查询的单元名");
            }
        }
        else{
            alert("请选择要查询的课程名");
        }
    }
})

app.controller('studentStatisticCtrl', function ($scope, $http) {

    //下拉框
    $scope.showClass = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowClass'
        }).then(function successCallback(response) {
            $scope.classInfo = response.data;
        }, function errorCallback(response) {
        });
    }
    $scope.showClass();
    $scope.showCourse = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowCourse'
        }).then(function successCallback(response) {
            $scope.courseInfo = response.data;
        }, function errorCallback(response) {
        });
    }
    $scope.showCourse();
    $scope.selectedUnit = function (courseId) {
        $scope.unitId = "";
        if (courseId != "") {
            $http({
                method: 'GET',
                url: 'http://localhost:5451/TrainApp/SelectedUnit',
                params: {
                    'courseId': courseId
                }
            }).then(function successCallback(response) {
                $scope.unitInfo = response.data;
            }, function errorCallback(response) {
                alert("查询失败");
            });
        }
        else { alert("错误"); }
    }
    $scope.queryFeedback = function (classId, unitId, courseId) {
        //console.log(courseId + " " + unitId);
        if (classId != "" && classId != null && classId != undefined) {
            if (courseId != "" && courseId != null && courseId != undefined) {
                if (unitId == "" || unitId == null || unitId == undefined) {
                    $http({
                        method: 'GET',
                        url: 'http://localhost:5451/TrainApp/QueryFeedbackOfCourse',
                        params: {
                            'classId': classId,
                            'courseId': courseId
                        }
                    }).then(function successCallback(response) {
                        alert("查询成功");
                        $scope.feedbackInfo = response.data;
                        if ($scope.feedbackInfo.length > 5) {
                            $scope.highFeedbackInfo = $scope.feedbackInfo.slice(0, 5);
                            $scope.lowFeedbackInfo = $scope.feedbackInfo.slice(-5);
                        }
                        else {
                            $scope.highFeedbackInfo = $scope.feedbackInfo;
                            $scope.lowFeedbackInfo = $scope.feedbackInfo;
                        }
                    }, function errorCallback(response) {
                        alert("查询失败");
                    });
                }
                else {
                    $http({
                        method: 'GET',
                        url: 'http://localhost:5451/TrainApp/QueryFeedback',
                        params: {
                            'classId': classId,
                            'unitId': unitId,
                            'courseId': courseId
                        }
                    }).then(function successCallback(response) {
                        alert("查询成功");
                        $scope.feedbackInfo = response.data;
                        if ($scope.feedbackInfo.length > 5) {
                            $scope.highFeedbackInfo = $scope.feedbackInfo.slice(0, 5);
                            $scope.lowFeedbackInfo = $scope.feedbackInfo.slice(-5);
                        }
                        else {
                            $scope.highFeedbackInfo = $scope.feedbackInfo;
                            $scope.lowFeedbackInfo = $scope.feedbackInfo;
                        }
                    }, function errorCallback(response) {
                        alert("查询失败");
                    });                    
                }
            }
            else {
                alert("请选择要查询的课程");
            }
        }
        else {
            alert("请选择要查询的班级");
        }
    }


})

app.controller('studentNumStatisticCtrl', function ($scope, $http) {

    //下拉框
    $scope.showClass = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowClass'
        }).then(function successCallback(response) {
            $scope.classInfo = response.data;
        }, function errorCallback(response) {
        });
    }
    $scope.showClass();
    $scope.showCourse = function () {
        $http({
            method: 'GET',
            url: 'http://localhost:5451/TrainApp/ShowCourse'
        }).then(function successCallback(response) {
            $scope.courseInfo = response.data;
        }, function errorCallback(response) {
        });
    }
    $scope.showCourse();
    $scope.selectedUnit = function (courseId) {
        $scope.unitId = "";
        if (courseId != "") {
            $http({
                method: 'GET',
                url: 'http://localhost:5451/TrainApp/SelectedUnit',
                params: {
                    'courseId': courseId
                }
            }).then(function successCallback(response) {
                $scope.unitInfo = response.data;
            }, function errorCallback(response) {
                alert("查询失败");
            });
        }
        else { alert("错误"); }
    }
    $scope.queryFeedback = function (classId, unitId, courseId) {
        //console.log(courseId + " " + unitId);
        if (classId != "" && classId != null && classId != undefined) {
            if (courseId != "" && courseId != null && courseId != undefined) {
                if (unitId == "" || unitId == null || unitId == undefined) {
                    $http({
                        method: 'GET',
                        url: 'http://localhost:5451/TrainApp/QueryNumOfCourse',
                        params: {
                            'classId': classId,
                            'courseId': courseId
                        }
                    }).then(function successCallback(response) {
                        alert("查询成功");
                        $scope.feedbackInfo = response.data;
                        if ($scope.feedbackInfo.length > 5) {
                            $scope.highFeedbackInfo = $scope.feedbackInfo.slice(0, 5);
                            $scope.lowFeedbackInfo = $scope.feedbackInfo.slice(-5);
                        }
                        else {
                            $scope.highFeedbackInfo = $scope.feedbackInfo;
                            $scope.lowFeedbackInfo = $scope.feedbackInfo;
                        }
                    }, function errorCallback(response) {
                        alert("查询失败");
                    });
                }
                else {
                    $http({
                        method: 'GET',
                        url: 'http://localhost:5451/TrainApp/QueryNum',
                        params: {
                            'classId': classId,
                            'unitId': unitId,
                            'courseId': courseId
                        }
                    }).then(function successCallback(response) {
                        alert("查询成功");
                        $scope.feedbackInfo = response.data;
                        if ($scope.feedbackInfo.length > 5) {
                            $scope.highFeedbackInfo = $scope.feedbackInfo.slice(0, 5);
                            $scope.lowFeedbackInfo = $scope.feedbackInfo.slice(-5);
                        }
                        else {
                            $scope.highFeedbackInfo = $scope.feedbackInfo;
                            $scope.lowFeedbackInfo = $scope.feedbackInfo;
                        }
                    }, function errorCallback(response) {
                        alert("查询失败");
                    });
                }
            }
            else {
                alert("请选择要查询的课程");
            }
        }
        else {
            alert("请选择要查询的班级");
        }
    }


})
