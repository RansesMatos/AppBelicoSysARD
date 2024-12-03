// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const { drop } = require("../../../../../../node_modules/cypress/types/lodash/index");

// Write your JavaScript code.

const button = document.querySelector('#Mant');

const disableButton = () => {
    console.log("va");
    button.disabled = true;
};

button.addEventListener('click', disableButton);
const button2 = document.querySelector('#Mant2');

const disableButton2 = () => {
    console.log("va");
    button2.disabled = true;
};

button2.addEventListener('click', disableButton2);
const button3 = document.querySelector('#Mant3');
const disableButton3 = () => {
    console.log("va");
    button3.disabled = true;
};

button3.addEventListener('click', disableButton3);

$(document).ready(function () {
    $('#IdArma').on('change', function () {
        let selectedItem = $(this).find(':selected');
        let selectedItemName = selectedItem.text();
        $('#IdArma').val(selectedItemName);
        $(this).attr('data-placeholder', selectedItemName);
    });
})

function searchName() {
    let name = document.getElementById("searchInput").value;

    // Update the field in the main view with the selected name
    document.getElementById("nameField").value = name;

    // Close the modal
    let modal = new bootstrap.Modal(document.getElementById("searchModal"));
    modal.hide();
}
function deleteAsignacion(id) {

    $.ajax({
        url: 'Delete',
        type: 'delete',
        data: {
            id: id
        },
        success: function (data) {
            let dropdown = $('#MilitarNo');
            dropdown.empty(); // Clear existing options

            // Add options to the dropdown
            $.each(data, function (index, item) {
                dropdown.append($('<option></option>').text(item.rangos + " " + item.nombres + " " + item.cedula).val(item.militarNo));
            });


            console.log(data)
        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown data: ' + error);
        }
    });
}
function UpdatePertrecho() {
    let description = document.getElementById("PertrechosDescripcion").value;
    //let description = document.getElementById("PertrechosDescripcion").;
    let cantidad = document.getElementById("Cantidad").value;
    let almacen = document.getElementById("IdAlmacen").value;

    $.ajax({
        url: 'PertrechoUpdate',
        type: 'PATCH',
        data: {
            idPertrechos: description,
            pertrechosDescripcion: description,
            cantidad: cantidad,
            idAlmacen: almacen
        },
        success: function (data) {
            console.log(data)
        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown data: ' + error);
        }
    });
}
function loadDropdownData() {
    let nombrefilter = document.getElementById("searchInput").value;
    let nombrefilter2 = document.getElementById("searchInput2").value;
    $.ajax({
        url: 'SearchPeople',
        type: 'GET',
        data: {
            carnet: nombrefilter,
            cedula: nombrefilter2
        },
        success: function (data) {
            let dropdown = $('#MilitarNo');
            dropdown.empty(); // Clear existing options

            // Add options to the dropdown
            $.each(data, function (index, item) {
                dropdown.append($('<option></option>').text(item.rangos + "  " + item.nombres + " - ( " + item.cedula + " )").val(item.militarNo));
            });

            console.log(data)
        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown data: ' + error);
        }
    });
}

function descargarPertrecho(button) {

    var noM = button.getAttribute('data-no-m');
    var idPertrechos = button.getAttribute('data-id-pertrechos');

    fetch(`/Asignacion/DescargarPertrecho?NoMilitar=${noM}&IdPertrecho=${idPertrechos}`,
    {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => {
        if (response.ok) {
            return response.json();
        }
        throw new Error('Error en la respuesta del servidor');
    })
    .then(data => {
        Swal.fire({
            icon: 'success',
            title: 'Descargado',
            text: 'El pertrecho se ha descargado exitosamente.'
        }).then(() => {
            location.reload();
        });
    })
    .catch(error => {
        console.error('Error:', error);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Ocurrió un error al descargar el pertrecho.'
        });
     });
}

function ValidarPertrechoMilitar(militarNo) { 

    try {
        const response = await fetch(`/Asignacion/ValidarPertrechoAsignacion?NoMilitar=${militarNo}`, {
            method: 'POST',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        if (!response.ok) throw new Error('Error en la respuesta del servidor');

        const data = await response.json();

        Swal.fire({
            icon: data.success ? 'success' : 'error',
            title: data.success ? 'Seleccionado' : 'Error',
            text: data.message || (data.success ? 'La asignación ha sido seleccionada exitosamente.' : 'No se pudo completar la selección.')
        });

    } catch (error) {
        console.error('Error:', error);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Ocurrió un error al seleccionar la asignación.'
        });
    }
}

function loadDropdownData2() {
    let nombrefilter = document.getElementById("searchInput").value;
    let nombrefilter2 = document.getElementById("searchInput2").value;
    $.ajax({
        url: 'SearchPeople',
        type: 'GET',
        data: {
            carnet: nombrefilter,
            cedula: nombrefilter2
        },
        success: function (data) {
            let dropdown = $('#MilitarNo');
            dropdown.empty(); // Clear existing options

            // Add options to the dropdown
            $.each(data, function (index, item) {
                dropdown.append($('<option></option>').text(item.rangos + " " + item.nombres + " - ( " + item.cedula + " )").val(item.militarNo));
            });
            console.log("Prueba",data)
        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown data: ' + error);
        }
    });
}
function loadDropdownData3() {
    let nombrefilter = document.getElementById("searchInput").value;
    let nombrefilter2 = document.getElementById("searchInput2").value;
    $.ajax({
        url: 'SearchPeople',
        type: 'GET',
        data: {
            carnet: nombrefilter,
            cedula: nombrefilter2
        },
        success: function (data) {
            let dropdown = $('#AsignacionNombreCert');
            dropdown.empty(); // Clear existing options

            // Add options to the dropdown
            $.each(data, function (index, item) {
                dropdown.append($('<option></option>').text(item.rangos + " " + item.nombres + " - ( " + item.cedula + " )").val(item.militarNo));
            });


            console.log(data)
        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown data: ' + error);
        }
    });
}
function loadDropdownData4() {
    let nombrefilter = document.getElementById("searchInput").value;
    let nombrefilter2 = document.getElementById("searchInput2").value;
    $.ajax({
        url: 'SearchPeople',
        type: 'GET',
        data: {
            carnet: nombrefilter,
            cedula: nombrefilter2
        },
        success: function (data) {
            let dropdown = $('#MilitarNo');
            dropdown.empty(); // Clear existing options

            // Add options to the dropdown
            $.each(data, function (index, item) {
                dropdown.append($('<option></option>').text(item.rangos + " " + item.nombres + " - ( " + item.cedula + " )").val(item.militarNo));
            });


            console.log(data)
        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown data: ' + error);
        }
    });
}
function loadDocData() {
    let idfilter = document.getElementById("MilitarNo").value;
    $.ajax({
        url: 'PeronaId',
        type: 'GET',
        data: {
            id: idfilter
        },
        success: function (data) {
            console.log("datos" + data)
            let documentId = $('#AsignacionDocumento');
            // Clear existing options
            document.getElementById("AsignacionDocumento1").textContent = 'Documento: ' + data;
            document.getElementById("AsignacionDocumento").textContent = data;
            documentId.val(data)
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
        }
    });
    $.ajax({
        url: 'PeronaIdrango',
        type: 'GET',
        data: {
            id: idfilter
        },
        success: function (datar) {
            console.log(datar)
            let rango = $('#Asignacionrango');
            rango.empty(); // Clear existing options
            document.getElementById("Asignacionrango1").textContent = 'Rango: ' + datar;
            document.getElementById("Asignacionrango").textContent = datar;
            rango.val(datar)
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
        }
    });
    $.ajax({
        url: 'PeronaIdNoMilitar',
        type: 'GET',
        data: {
            id: idfilter
        },
        success: function (datan) {
            console.log(datan)
             let noMilitar = $('#AsignacionNoRango');
            let mNombre = $('#AsignacionNombre');
            document.getElementById("AsignacionNoRango1").textContent = 'NoMilitar: ' + datan.militarNo;            
            document.getElementById("AsignacionNombre").textContent = datan.nombres;
            document.getElementById("AsignacionNoRango").textContent = datan.militarNo;
            mNombre.val(datan.nombres)
            noMilitar.val(datan.militarNo)
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
        }
    });

}
function loadDocDataOrden() {
    let idfilter = document.getElementById("AsignacionNombre").value;
    $.ajax({
        url: 'PeronaId',
        type: 'GET',
        data: {
            id: idfilter
        },
        success: function (data) {
            console.log(data)
            let documentId = $('#AsignacionDocumento1');
            documentId.empty(); // Clear existing options
            document.getElementById("AsignacionDocumento").textContent = 'Documento: ' + data;
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
        }
    });
    $.ajax({
        url: 'PeronaIdrango',
        type: 'GET',
        data: {
            id: idfilter
        },
        success: function (datar) {
            console.log(datar)
            let rango = $('#Asignacionrango1');
            rango.empty(); // Clear existing options
            document.getElementById("Asignacionrango").textContent = 'Rango: ' + datar;
            rango.val(datar)
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
        }
    });
    $.ajax({
        url: 'PeronaIdNoMilitar',
        type: 'GET',
        data: {
            id: idfilter
        },
        success: function (datan) {
            console.log(datan)
            let noMilitar = $('#AsignacionNoRango1');
            noMilitar.empty(); // Clear existing options
            document.getElementById("AsignacionNoRango").textContent = 'NoMilitar: ' + datan.militarNo;
            noMilitar.val(datan.nombres)
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
        }
    });

}
function loadDocDataAsigCert() {
    let idfilter = document.getElementById("AsignacionNombreCert").value;
    $.ajax({
        url: 'PeronaId',
        type: 'GET',
        data: {
            id: idfilter
        },
        success: function (data) {
            console.log(data)
            let documentId = $('#AsignacionNombreCert');
            documentId.empty(); // Clear existing options
            document.getElementById("AsignacionDocumento").textContent = 'Documento: ' + data;
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
        }
    });
    $.ajax({
        url: 'PeronaIdrango',
        type: 'GET',
        data: {
            id: idfilter
        },
        success: function (datar) {
            console.log(datar)
            let rango = $('#Asignacionrango1');
            rango.empty(); // Clear existing options
            document.getElementById("Asignacionrango").textContent = 'Rango: ' + datar;
            rango.val(datar)
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
        }
    });
    $.ajax({
        url: 'PeronaIdNoMilitar',
        type: 'GET',
        data: {
            id: idfilter
        },
        success: function (datan) {
            console.log(datan)
            let noMilitar = $('#AsignacionNoRango1');
            noMilitar.empty(); // Clear existing options
            document.getElementById("AsignacionNoRango").textContent = 'NoMilitar: ' + datan.militarNo;
            noMilitar.val(datan.nombres)
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
        }
    });

}
function showNotification(message) {
    const notification = document.getElementById('notification');
    notification.innerHTML = message;
    notification.style.display = 'block';

    // Automatically hide the notification after a few seconds (optional)
    setTimeout(() => {
        notification.style.display = 'none';
    }, 5000); // Hide after 5 seconds (adjust as needed)
}
function loadDropdownArma() {
    let nombrefilter = document.getElementById("searchArmaInput").value;
    $.ajax({
        url: 'SearchArma',
        type: 'GET',
        data: {
            armaserial: nombrefilter
        },
        success: function (data) {
            console.log(data)
            let idarma = $('#IdArma1');
            idarma.empty(); // Clear existing options    
            if (data.armaCalibre == null) {
                document.getElementById("calibre").textContent = 'Calibre:  Arma no Existe';
                document.getElementById("armaTipo").textContent = 'Tipo: Arma no Existe';
                document.getElementById("armaMarca").textContent = 'Marca: Arma no Existe';

            } else {
                document.getElementById("calibre").textContent = 'Calibre: ' + data.armaCalibre;
                document.getElementById("armaTipo").textContent = 'Tipo: ' + data.taNombre;
                document.getElementById("armaMarca").textContent = 'Marca: ' + data.armaMarcaDescripcion;
                idarma.val(data.idArma)
            }
        },
        error: function (xhr, status, error) {
            document.getElementById("calibre").textContent = 'Calibre:  Arma no Existe';
            document.getElementById("armaTipo").textContent = 'Tipo: Arma no Existe';
            document.getElementById("armaMarca").textContent = 'Marca: Arma no Existe';
            console.error('Error loading dropdown data: ' + error);
        }
    });
}
function loadDropdownArmaupdated() {
    let nombrefilter = document.getElementById("searchArmaInput").value;
    $.ajax({
        url: 'SearchArma',
        type: 'GET',
        data: {
            armaSerial: nombrefilter
        },
        success: function (data) {
            console.log(data)
            let idarma = $('#IdArma1');
            let dropdown = document.getElementById('IdTipoArmaU');
            if (data.taNombre.length > 0) {
                // Sustituye el primer valor en la lista

                // dropdown.find('option').addValue(data.taNombre)
                $('#IdTipoArmaU option:first').text(data.taNombre).val(data.taNombre);
                $('#IdArmaMarca option:first').text(data.armaMarcaDescripcion).val(data.armaMarcaDescripcion);
                $('#ArmaCalibre option:first').text(data.armaCalibre).val(data.armaCalibre);

            }
            idarma.empty(); // Clear existing options     
            //dropdown.append($('<option>', data.taNombre));
            idarma.val(data.idArma)

        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown data: ' + error);
        }
    });
}
function ExportPDFCertificate() {
    let nombrefilter = document.getElementById("searchArmaInput").value;
    $.ajax({
        url: 'ExportPDf',
        type: 'GET',
        data: {
            armaserial: nombrefilter
        },
        success: function (data) {
            console.log(data)
            let idarma = $('#IdArma1');
            let dropdown = document.getElementById('IdTipoArmaU');
            idarma.empty(); // Clear existing options     
            dropdown.append($('<option></option>').text('aguas').val(data.idarma));
            idarma.val(data.idArma)

        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown data: ' + error);
        }
    });
}

function confirmarAccion({ callBackAceptar, callbackCancelar, titulo }) {
    Swal.fire({
        title: titulo || 'Confirmas que esta es la Accion que deseas Realizar.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        focusConfirm: true,
    }).then((resultado) => {
        if (resultado.isConfirmed) {
            callBackAceptar();
        } else if (callbackCancelar) {
            callbackCancelar();
        }
    })

}
function borrarAsignacion(asignacion) {
    confirmarAccion({
        callBackAceptar: () => {
            deleteAsignacion(asignacion);
        },
        callbackCancelar: () => { },
        titulo: 'Desea Eliminar la Asignacion?'
    })

}

/**function saveOrder(){
    const btnSaveOrder = $('#btnSaveOrder');
    const formData = $("#formOrdenMaterial").serialize();
 
    const url = "/Asignacion/saveOrder";
    btnSaveOrder.click((e) => {
        e.PreventDefault();
        $.post(url, formData, (resp) => console.log("Registro exitoso"))
    });
}**/









