namespace MetroDigger.Utils
{
    interface IAssembler<T, TDto>
    {
        TDto GetDto(T plain);
        T GetPlain(TDto dto);
    }
}
