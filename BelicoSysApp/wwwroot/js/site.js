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


function loadDropdownData() {
    let nombrefilter =  document.getElementById("searchInput").value;
    $.ajax({
        url: 'SearchPeople',
        type: 'GET',
        data: {
            nombre: nombrefilter},
        success: function (data) {
            let dropdown = $('#MilitarNo');
            dropdown.empty(); // Clear existing options
            
            // Add options to the dropdown
            $.each(data, function (index, item) {
                dropdown.append($('<option></option>').text(item.text).val(item.value));                
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
    $.ajax({
        url: 'SearchPeople',
        type: 'GET',
        data: {
            nombre: nombrefilter
        },
        success: function (data) {
            let dropdown = $('#AsignacionNombre');
            dropdown.empty(); // Clear existing options

            // Add options to the dropdown
            $.each(data, function (index, item) {
                dropdown.append($('<option></option>').text(item.text).val(item.value));
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
            let documentId = $('#AsignacionDocumento');
            documentId.empty(); // Clear existing options
            documentId.val(data)
            console.log(data)
        },
        error: function (xhr, status, error) {
            console.error('Error Cargando Cedula data: ' + error);
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




