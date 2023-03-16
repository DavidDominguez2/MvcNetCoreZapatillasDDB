using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreZapatillas.Data;
using MvcNetCoreZapatillas.Models;
using System.Data;

#region SQL SERVER
//VUESTRO PROCEDIMIENTO DE PAGINACION DE IMAGENES DE ZAPATILLAS
//CREATE PROCEDURE SP_PAGINACION_IMGSZAPATILLAS
//(@POSICION INT, @IDPROC INT, @NUMEROREGISTROS INT OUT)
//	AS
//        SELECT @NUMEROREGISTROS = COUNT(IDIMAGEN)
//								  FROM IMAGENESZAPASPRACTICA
//								  WHERE IDPRODUCTO = @IDPROC

//		SELECT* FROM(SELECT CAST(
//		ROW_NUMBER() OVER(ORDER BY IDIMAGEN)AS INT) AS POSICION,
//        IDIMAGEN, IDPRODUCTO, IMAGEN
//		FROM IMAGENESZAPASPRACTICA
//		WHERE IDPRODUCTO = @IDPROC) AS QUERY
//		WHERE QUERY.POSICION >= @POSICION AND QUERY.POSICION<(@POSICION +1)
//GO
#endregion

namespace MvcNetCoreZapatillas.Repositories {
    public class RepositoryZapatillas {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context) {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillasAsync() {
            return await this.context.Zapatillas.ToListAsync();
        }

        public async Task<Zapatilla> FindZapatillaAsync(int idZapatilla) {
            return await this.context.Zapatillas.Where(x => x.IdProducto == idZapatilla).FirstOrDefaultAsync();
        }

        public async Task<PaginacionImagenesZapatilla> GetImagenesZapatillaAsync(int posicion, int idZapatilla) {
            string sql = "SP_PAGINACION_IMGSZAPATILLAS @POSICION, @IDPROC, @NUMEROREGISTROS OUT";
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            SqlParameter pamzapatilla = new SqlParameter("@IDPROC", idZapatilla);
            SqlParameter pamnumregistros = new SqlParameter("@NUMEROREGISTROS", -1);
            pamnumregistros.Direction = ParameterDirection.Output;

            var consulta = this.context.ImagenesZapatillas.FromSqlRaw(sql, pamposicion, pamzapatilla, pamnumregistros);
            List<ImagenZapatilla> images = await consulta.ToListAsync();
            int registros = (int)pamnumregistros.Value;

            return new PaginacionImagenesZapatilla() {
                NumeroRegistros = registros,
                ImagenesZapatilla = images
            };
        }
    }
}
