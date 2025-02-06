using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public equiposController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipo = (from e in _equiposContexto.equipos
                                           join t in _equiposContexto.tipo_equipo
                                                on e.tipo_equipo_id equals t.id_tipo_equipo
                                           join m in _equiposContexto.marcas
                                                on e.marca_id equals m.id_marcas
                                           join es in _equiposContexto.estados_equipo
                                                on e.estado_equipo_id equals es.id_estados_equipo
                                           select new
                                           {
                                                e.id_equipos,
                                                e.nombre,
                                                e.descripcion,
                                                e.tipo_equipo_id,
                                                tipo_equipo = t.descripcion,
                                                e.marca_id,
                                                marca = m.nombre_marca,
                                                e.estado_equipo_id,
                                                estado_equipo = es.descripcion,
                                                detalle = $"Tipo: {t.descripcion}, Marca : {m.nombre_marca}, Estado Equipo : {es.descripcion}",
                                                e.estado
                                           }).OrderBy(resultado => resultado.estado_equipo_id).ToList();
            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }



        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] equipos equipo)
        {
            try
            {
                _equiposContexto.equipos.Add(equipo);
                _equiposContexto.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            equipos? equipoActual = (from e in _equiposContexto.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();



            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();


            return Ok(equipoModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipos(int id)
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();



            if (equipo == null)
            {
                return NotFound();
            }



            _equiposContexto.equipos.Attach(equipo);
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();



            return Ok(equipo);
        }
    }

}