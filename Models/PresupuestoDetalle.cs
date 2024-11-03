namespace Models;

    public class PresupuestoDetalle
    {
        public Producto? Producto {get; set;}
        public int Cantidad {get; set;}


        public void CargarProducto(Producto prod)
        {
            Producto = prod;
        }

    }
