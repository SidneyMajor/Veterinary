// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function validFiles() {
    var validFilesTypes = ["bmp", "png", "jpg", "jpeg"];

    var file = document.getElementById("image");
    var isValidFile = false;
    file.onchange = function () {
        var path = file.value;
        var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

        for (var i = 0; i < validFilesTypes.length; i++) {
            if (ext == validFilesTypes[i]) {
                isValidFile = true;
                break;
            }
            else {
                isValidFile = false;
            }
        }

        if (!isValidFile) {

            swal("System warning", "Invalid File. Please upload a File with" +
                " extension:\n" + validFilesTypes.join(", "), "warning");
            file.value = "";
        }
        else {
            var result = document.getElementById("imgpreview");
            result.src = URL.createObjectURL(event.target.files[0]);
        }
    }
}

function imageClick() {
    image = document.getElementById("imgpreview");
    image.onclick = function () {
        $("#image").click();
    }
}

function specialtychange() {
    // disable the doctor DropDownList
    var doctor = document.getElementById('doctorid').ej2_instances[0];
    var specialty = document.getElementById('specialtyid').ej2_instances[0];
    doctor.enabled = true;
    //frame the query based on selected value in specialty DropDownList.
    var tempQuery = new ej.data.Query().where('SpecialtyID', 'equal', specialty.value);
    // set the framed query based on selected value in specialty DropDownList.
    doctor.query = tempQuery;
    // set null value to doctor DropDownList text property
    doctor.text = null;
    //  bind the property changes to doctor DropDownList
    doctor.dataBind();
}

//ajaxSubmit = form => {
//    alert("teste4");
//    try {
       
//        $.ajax({
//            type: 'POST',
//            url=form.action,
//            data: new FormData(form),
//            dataType: 'JSON',
//            processData: false,
//            contentType: false,
           
//        success: function (res) {
//            alert("teste3");
//            if (res.isValid) {
//                $("#viewAppointment").html(res.html);
//                $("#myModal .modal-body").html('');
//                $("#myModal .modal-title").html(' ');
//                $("#myModal").modal('hide');
//            }
//            else {
//                alert("teste2");
//                $("#myModal .modal-body").html(res.html);
//                $("#myModal .modal-title").html('<i class="fas fa-comment-medical text-info"></i> Add Appointment');
//                $("#myModal").modal('show');
//            }
//        },
//            error: function (err) {
//                alert("teste1");
//            alert(res.isValid);
//            console.log(err);
//        }
//    })

//} catch (e) {
//    console.log(e);
//}

////To privent default submit event
//return false;
//}
