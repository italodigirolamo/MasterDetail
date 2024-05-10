using MasterDetail.Data;
using MasterDetail.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterDetail.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public PedidoController(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<ActionResult> Index()
        {
            var pedidos = await _dbContext.Pedidos
                .Include(p => p.Cliente)
                .ToListAsync();

            return View(pedidos);
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Clientes = await _dbContext.Clientes.ToListAsync();
           // ViewBag.Clientes = await _dbContext.Clientes.ToListAsync();
             ViewBag.Productos = await _dbContext.Productos.ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Pedido pedido, int[] productoId, int[] cantidades)
        {
            var cli = await _dbContext.Clientes.FirstOrDefaultAsync(d => d.ClienteId == pedido.ClienteId);

            if (cli != null)
            {
                pedido.Cliente = cli;
            }

            foreach (var item in productoId)
            {
                var producto = await _dbContext.Productos.FindAsync(item);
                if (producto != null)
                {
                    pedido.Detalles.Add(new PedidoDetalle
                    {
                        PedidoId = item,
                        Cantidad = cantidades[Array.IndexOf(productoId, item)],
                        Producto = producto
                    });
                }
            }
            _dbContext.Pedidos.Add(pedido);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Details(int id)
        {
            var pedido = await _dbContext.Pedidos
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .Include(p => p.Cliente)
            .FirstOrDefaultAsync(p => p.PedidoId == id);

            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pedido = await _dbContext.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewBag.Clientes = _dbContext.Clientes.ToList();
            return View(pedido);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pedido pedido)
        {
            if (id != pedido.PedidoId)
                return NotFound();

            _dbContext.Update(pedido);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _dbContext.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.PedidoId == id);

            if (pedido == null)
            {
                return NotFound();
            }

            var detallesPedido = await _dbContext.PedidosDetalle 
                .Include(d => d.Producto)
                .Where(d => d.PedidoId == id)
                .ToListAsync();

            ViewBag.DetallesPedido = detallesPedido;
            return View(pedido);


        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm (int id)
        {
            var pedido = await _dbContext.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _dbContext.Pedidos.Remove(pedido);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public IActionResult EditDetalle(int id)
        {
            var detalle = _dbContext.PedidosDetalle.Find(id);
            if (detalle == null)
            {
                return NotFound();
            }
            var producto = _dbContext.Productos.FirstOrDefault(p => p.ProductoId == detalle.ProductoId);
            if (producto == null)
            {
                return NotFound();
            }
            ViewBag.Producto = producto.Precio;
            ViewBag.Productos = _dbContext.Productos.ToList();

            return View(detalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditDetalle(int id, PedidoDetalle detalle)
        {
            var existingDetalle = _dbContext.PedidosDetalle
                .Include(d => d.Pedido)
                .FirstOrDefault(d => d.PedidoDetalleId == id);

            if (existingDetalle == null)
            {
                return NotFound();
            }

            if (id != detalle.PedidoDetalleId)
            {
                return NotFound();
            }

            existingDetalle.Cantidad = detalle.Cantidad;

            _dbContext.Update(existingDetalle);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { Id = existingDetalle.PedidoId});

        }

        public async Task<IActionResult> DeleteDetalle(int id)
        {
            var detalle = await _dbContext.PedidosDetalle
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d=> d.PedidoDetalleId == id);

            if (detalle == null)
                return NotFound();

            return View(detalle);


        }

        [HttpPost, ActionName("DeleteDetalle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDetalleConfirm (int id)
        {

            var detail = await _dbContext.PedidosDetalle.FindAsync(id);
                {
                //var detail = await _dbContext.PedidosDetalle.FindAsync(id);
                ////var detail = await _dbContext.PedidosDetalle.FindAsync($"{id}");

                if (detail == null)
                    return NotFound();

                _dbContext.PedidosDetalle.Remove(detail);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof (Details), new { Id = detail.PedidoId });
            }
        }


    }

}
