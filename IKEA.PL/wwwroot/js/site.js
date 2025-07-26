        
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.






// document.addEventListener("DOMContentLoaded", function () {

//var searchBtn = document.getElementById("searchInp");

//function searchEmployees() {
//    debugger

//    var searchTerm = $(`searchInp`).val();

//    $.ajax({
//        url: `https://localhost:7176/Employee/Index`,
//        type: "GET",
//        data: { searchTerm: searchTerm },
//        success: function (data) {
//            $("#employeeList").html(data);
//        },
//        error: function () {
//            alert("Error fetching employees.");
//        }
//    });
//}

//     searchEmployees();

//searchBtn.addEventListener("keyup", function () {

//    var xhr = new XMLHttpRequest();
//    xhr.onreadystatechange = function () {
//        if (xhr.readyState === 4 && xhr.status === 200) {

//            // Update the employee list with the response
//            document.getElementById("employeeList").innerHTML = xhr.responseText;
//        }
//    };
//    xhr.open("GET", `https://localhost:7176/Employee/Index`);
//    xhr.open();
//})



document.addEventListener("DOMContentLoaded", function () {
    var SearchInp = document.getElementById("searchInp");
    if (!SearchInp) return;

    SearchInp.addEventListener("keyup", function () {
        var Searchvalue = SearchInp.value;

        var xhr = new XMLHttpRequest();

        xhr.open("GET", `/Employee?search=${Searchvalue}`);

        xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");


        xhr.send();

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    document.getElementById("results").innerHTML = xhr.responseText;
                } else {
                    alert("Something went wrong");
                }
            }
        };
    });
});



























    //var searchInp = document.getElementById("searchInp");
    //if (searchInp) {
    //    searchInp.addEventListener("keyup", function () {




    //        searchvalue = searchInp.value;

    //        var xhr = new XMLHttpRequest();

    //        xhr.open("Get", `https://localhost:7176/Employee/Index?search=${searchvalue}`);

    //        xhr.send();

    //        xhr.onreadystatechange = function () {
    //            if (xhr.readyState == XMLHttpRequest.DONE) {

    //                if (xhr.status == 200) {

    //                    document.getElementById("employeeList").innerHTML = xhr.responseText;
    //                }


    //                else {
    //                    alert(`SomeThing Else Other Than 200 Was Returned`);
    //                }
    //            }

    //        };
    //    })


    //};





  