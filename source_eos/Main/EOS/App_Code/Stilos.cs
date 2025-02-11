using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Descripción breve de Class1
/// </summary>
public static class Stilos
{
    //public Stilos()
    //{
    //    //
    //    // TODO: Agregar aquí la lógica del constructor
    //    //
    //}

    /// <summary>
    /// Letra= Arial
    /// tamaño 8
    /// Sombreado verde corporativo
    /// color texto negro.
    /// </summary>
    /// <returns></returns>
    public static Style cuerpoInforme()
    {
        Style st = new Style();
        st.Font.Bold = false;
        st.Font.Name = "Arial";
        st.Font.Size = 8;
        //st.BackColor = System.Drawing.Color.FromArgb(40, 150, 40); //poner el verde corporativo
        st.ForeColor = System.Drawing.Color.Black;
        return st;
    }

    /// <summary>
    /// Letra= Arial Negrita
    /// tamaño 8
    /// Sombreado verde corporativo
    /// color texto blanco.
    /// </summary>
    /// <returns></returns>
    public static Style cabeceraColumnas()
    {
        Style st = new Style();
        st.Font.Bold = true;
        st.Font.Name = "Arial";
        st.Font.Size = 8;
        st.BackColor = System.Drawing.Color.FromArgb(40, 150, 40); //poner el verde corporativo
        st.ForeColor = System.Drawing.Color.White;
        return st;
    }

    /// <summary>
    /// Centrado
    /// Arial negrita, Tamaño 12
    /// Color texto negro
    /// </summary>
    /// <returns></returns>
    public static Style tituloInforme()
    {
        Style st = new Style();
        st.Font.Bold = true;
        st.Font.Name = "Arial";
        st.Font.Size = 12;
        //st.BackColor = System.Drawing.Color.White; //poner el verde corporativo
        st.ForeColor = System.Drawing.Color.Black;
        return st;
    }

    /// <summary>
    /// Centrado
    /// Arial normal, Tamaño 10
    /// Color texto gris oscuro
    /// </summary>
    /// <returns></returns>
    public static Style segundaLineaEncabezado()
    {
        Style st = new Style();
        st.Font.Bold = false;
        st.Font.Name = "Arial";
        st.Font.Size = 10;
        //st.BackColor = System.Drawing.Color.White;
        st.ForeColor = System.Drawing.Color.FromArgb(30, 30, 30); //gris oscuro
        return st;
    }

    /// <summary>
    /// Derecha
    /// Arial narrow, Tamaño 7
    /// Color texto gris oscuro
    /// </summary>
    /// <returns></returns>
    public static Style numeroPagina()
    {
        Style st = new Style();
        st.Font.Bold = false;
        st.Font.Name = "Arial Narrow";
        st.Font.Size = 7;
        //st.BackColor = System.Drawing.Color.White; //poner el verde corporativo
        st.ForeColor = System.Drawing.Color.FromArgb(30, 30, 30); //gris oscuro
        return st;
    }

    /// <summary>
    /// Izquierda
    /// Arial narrow, Tamaño 7
    /// Color texto gris oscuro
    /// </summary>
    /// <returns></returns>
    public static Style leyendaAgencia()
    {
        Style st = new Style();
        st.Font.Bold = false;
        st.Font.Name = "Arial Narrow";
        st.Font.Size = 7;
        //st.BackColor = System.Drawing.Color.White; //poner el verde corporativo
        st.ForeColor = System.Drawing.Color.FromArgb(30, 30, 30);
        return st;
    }

    /// <summary>
    /// Derecha
    /// Arial narrow, Tamaño 7
    /// Color texto gris oscuro
    /// </summary>
    /// <returns></returns>
    public static Style fechaHoraImpresionEnPie()
    {
        Style st = new Style();
        st.Font.Bold = false;
        st.Font.Italic = true;
        st.Font.Name = "Arial Narrow";
        st.Font.Size = 7;
        //st.BackColor = System.Drawing.Color.White; //poner el verde corporativo
        st.ForeColor = System.Drawing.Color.FromArgb(30, 30, 30);
        return st;
    }

    /// <summary>
    ///Filtro aplicado:
    /// En la primera hoja2 del libro, como “Datos informe”
    /// Arial norma, tamaño 12
    /// Izquierda, una línea por filtro 
    /// “Unidad: Atención primaria”…
    /// </summary>
    /// <returns></returns>
    public static Style filtroAplicado()
    {
        Style st = new Style();
        st.Font.Bold = false;
        st.Font.Name = "Arial";
        st.Font.Size = 12;
        //st.BackColor = System.Drawing.Color.FromArgb(40, 150, 40); //poner el verde corporativo
        st.ForeColor = System.Drawing.Color.Black;
        return st;
    }

    /// <summary>
    /// Texto en negrita
    /// </summary>
    /// <returns></returns>
    public static Style negrita()
    {
        Style st = new Style();
        st.Font.Bold = true;
        return st;
    }

    /// <summary>
    /// Texto en cursiva
    /// </summary>
    /// <returns></returns>
    public static Style cursiva()
    {
        Style st = new Style();
        st.Font.Italic = true;
        return st;
    }
}
