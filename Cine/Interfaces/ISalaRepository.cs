namespace Cine.Interfaces
{
    public interface ISalaRepository
    {
        /// <summary>
        /// Busca la sala con un identificador dado.
        /// </summary>
        /// <param name="id">Id. de la sala a buscar</param>
        /// <returns>Sala con el identificador dado o null en caso de que no se encuentre.</returns>
        Sala Read(long id);
    }
}
