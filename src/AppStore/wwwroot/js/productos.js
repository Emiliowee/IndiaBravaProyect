$(document).ready(function() {
    inicializarUI();
    configurarEventos();
    
    $('#reporteModal').on('shown.bs.modal', function () {
        inicializarDatepickers();
    });
});

function inicializarUI() {
    $('.dropdown-toggle').dropdown();
    inicializarDatepickers();
    inicializarInputsNumericos();
}

function configurarEventos() {
    $('#optionsButton').click(toggleOptionsMenu);
    $(document).click(cerrarOptionsMenu);
    $('#guardarProducto').click(guardarProducto);
    $('#generarReporte').click(generarReporte);
    $('#guardarAgregarStock').click(agregarStock);
    $('#guardarCambios').click(guardarCambiosProducto);
}

function toggleOptionsMenu(event) {
    event.stopPropagation();
    $('#optionsMenu').toggle();
}

function cerrarOptionsMenu(event) {
    if (!$('#optionsButton').is(event.target) && !$('#optionsMenu').is(event.target)) {
        $('#optionsMenu').hide();
    }
}

function mostrarModalNuevoProducto() {
    $('#nuevoProductoForm')[0].reset();
    $('#nuevoProductoModal').modal('show');
}

function guardarProducto() {
    var formData = new FormData($('#nuevoProductoForm')[0]);
    $.ajax({
        url: '/Productos/Crear',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function(result) {
            if (result.success) {
                $('#nuevoProductoModal').modal('hide');
                location.reload();
            } else {
                mostrarError(result);
            }
        },
        error: function(xhr, status, error) {
            alert('Error al guardar el producto: ' + error);
        }
    });
}

function editarProducto(id) {
    $.ajax({
        url: '/Productos/ObtenerProducto/' + id,
        type: 'GET',
        success: function(producto) {
            llenarFormularioEdicion(producto);
            $('#productoModal').modal('show');
        },
        error: function(xhr, status, error) {
            alert('Error al obtener el producto: ' + error);
        }
    });
}

function llenarFormularioEdicion(producto) {
    $('#productoModalLabel').text('Editar Producto');
    $('#IDProducto').val(producto.idProducto);
    $('#productoForm #Nombre').val(producto.nombre);
    $('#productoForm #Precio').val(producto.precio);
    $('#productoForm #ContenidoNeto').val(producto.contenidoNeto);
    $('#productoForm #UnidadMedida').val(producto.unidadMedida);
    $('#productoForm #Stock').val(producto.stock);
}

function confirmarEliminarProducto(id) {
    if (confirm('¿Está seguro de que desea eliminar este producto?')) {
        eliminarProducto(id);
    }
}

function eliminarProducto(id) {
    $.post('/Productos/Eliminar/' + id, function(result) {
        if (result.success) {
            location.reload();
        } else {
            alert('Error al eliminar el producto: ' + result.error);
        }
    });
}

function mostrarModalAgregarStock() {
    $('#agregarStockForm')[0].reset();
    $('#agregarStockModal').modal('show');
}

function generarReporte() {
    $('#reporteModal').modal('show');
}

function enviarReporte() {
    var fechaInicial = $('#fechaInicial').val();
    var fechaFinal = $('#fechaFinal').val();

    if (!fechaInicial || !fechaFinal) {
        alert('Por favor, seleccione ambas fechas.');
        return;
    }

    $.ajax({
        url: '/Productos/GenerarReporte',
        type: 'POST',
        data: {
            fechaInicial: fechaInicial,
            fechaFinal: fechaFinal
        },
        success: function(result) {
            if (result.success) {
                alert('Reporte generado con éxito. Se ha descargado el archivo.');
                $('#reporteModal').modal('hide');
            } else {
                alert('Error al generar el reporte: ' + (result.error || 'Error desconocido'));
            }
        },
        error: function(xhr, status, error) {
            alert('Error al generar el reporte: ' + error);
        }
    });
}

function agregarStock() {
    var productoId = $('#productoId').val();
    var cantidadStock = $('#cantidadStock').val();

    if (!productoId || !cantidadStock) {
        alert('Por favor, complete todos los campos.');
        return;
    }

    $.ajax({
        url: '/Productos/AgregarStock',
        type: 'POST',
        data: {
            productoId: productoId,
            cantidadStock: cantidadStock
        },
        success: function(result) {
            if (result.success) {
                $('#agregarStockModal').modal('hide');
                location.reload();
            } else {
                alert('Error al agregar stock: ' + (result.error || 'Error desconocido'));
            }
        },
        error: function(xhr, status, error) {
            alert('Error al agregar stock: ' + error);
        }
    });
}

function inicializarDatepickers() {
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        language: 'es',
        autoclose: true,
        todayHighlight: true,
        container: '#reporteModal'
    });
}

function mostrarError(result) {
    let mensaje = 'Error al procesar la solicitud: ';
    if (result.modelErrors && result.modelErrors.length > 0) {
        mensaje += result.modelErrors.join(', ');
    } else if (result.error) {
        mensaje += result.error;
    } else {
        mensaje += 'Error desconocido';
    }
    alert(mensaje);
}

function guardarCambiosProducto() {
    var formData = new FormData($('#productoForm')[0]);
    $.ajax({
        url: '/Productos/Editar',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function(result) {
            if (result.success) {
                $('#productoModal').modal('hide');
                location.reload();
            } else {
                mostrarError(result);
            }
        },
        error: function(xhr, status, error) {
            alert('Error al guardar los cambios: ' + error);
        }
    });
}

function inicializarInputsNumericos() {
    $('input[type="number"]').attr('min', '0');
    $('#Precio, #ContenidoNeto').attr('step', '0.01');
    $('#Stock').attr('step', '1');
}

