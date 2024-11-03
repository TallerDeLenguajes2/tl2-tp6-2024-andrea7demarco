namespace Models;
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public string? NombreDestinatario { get; set; }

        public string? FechaCreacion {get; set; }
        private List<PresupuestoDetalle> Detalle { get; set; } = new List<PresupuestoDetalle>();

        public void AgregarProductos(Producto prod, int cant)
        {
            PresupuestoDetalle pd = new PresupuestoDetalle
            {
                Producto = prod,
                Cantidad = cant
            };
            Detalle.Add(pd);
        }

        public int MontoPresupuesto() =>
            Detalle.Sum(pd => pd.Cantidad * pd.Producto.Precio);

        public double MontoPresupuestoConIva() =>
            MontoPresupuesto() * 1.21;

        public int CantidadProductos() =>
            Detalle.Sum(pd => pd.Cantidad);
    }

