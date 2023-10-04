// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.
const valuesList = [];
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


function loadDropdownData() {
    let nombrefilter =  document.getElementById("searchInput").value;
    let nombrefilter2 =  document.getElementById("searchInput2").value;
    $.ajax({
        url: 'SearchPeople',
        type: 'GET',
        data: {
            nombre: nombrefilter,
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

function loadDropdownData2() {
    let nombrefilter = document.getElementById("searchInput").value;
    let nombrefilter2 = document.getElementById("searchInput2").value;
    $.ajax({
        url: 'SearchPeople',
        type: 'GET',
        data: {
            nombre: nombrefilter,
            cedula: nombrefilter2
        },
        success: function (data) {
            let dropdown = $('#AsignacionNombre');
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
            document.getElementById("calibre").textContent = 'Calibre: ' + data.armaCalibre;
            document.getElementById("armaTipo").textContent = 'Tipo: ' + data.taNombre;
            document.getElementById("armaMarca").textContent = 'Marca: ' + data.armaMarcaDescripcion;
            idarma.val(data.idArma)
        },
        error: function (xhr, status, error) {
            console.error('Error loading dropdown data: ' + error);
        }
    });
}

function addValue() {    
    const enteredValue = document.getElementById('IdPertrechos').value;
    valuesList.push(enteredValue);
    updateValuesTable();
};
function removeValue(index) {
    valuesList.splice(index, 1);
    updateValuesTable();
};

function updateValuesTable() {
    
    const valuesTableContainer = document.getElementById('valuesTable');
    valuesTableContainer.innerHTML = '<h3>Pertrechos asignados:</h3>';

    if (valuesList.length === 0) {
        valuesTableContainer.innerHTML += '<p>No values added yet.</p>';
    } else {
        const table = document.createElement('table');
        table.className = 'table table-bordered';
        const tbody = document.createElement('tbody');

        valuesList.forEach((value, index) => {
            const row = document.createElement('tr');
            const indexCell = document.createElement('td');
            indexCell.textContent = index + 1;
            const valueCell = document.createElement('td');
            valueCell.textContent = value;
            const inputVal = document.createElement('input');
            inputVal.type = 'number';
            const removeCell = document.createElement('td');
            const btnRemove = document.createElement('button');
            btnRemove.textContent = 'Remover';
            btnRemove.className = 'btn btn-danger btn-sm';
            btnRemove.style.color = 'Black';
            btnRemove.onclick = () => removeValue(index);
            removeCell.appendChild(btnRemove);


            row.appendChild(btnRemove);
            row.appendChild(valueCell);
            row.appendChild(inputVal);
            
            tbody.appendChild(row);
        });

        table.appendChild(tbody);
        valuesTableContainer.appendChild(table);
    }

   
}


function confirmarAccion({ callBackAceptar, callbackCancelar, titulo })
{
    Swal.fire({
        tittle: titulo || 'Confirmas que esta es la Accion que deseas Realizar.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor:'#3085d6',
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

function borrarAsignacion(asignacion)
{
    confirmarAccion({
        callBackAceptar: () => {
            deleteAsignacion(asignacion);
        },
        callbackCancelar: () => { },
        titulo: 'Desea Eliminar la Asignacion?'
    })

}








