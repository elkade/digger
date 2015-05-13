namespace MetroDigger.Gameplay.Entities.Terrains
{
    /// <summary>
    /// Wolny teren, na który dostęp mają wszystkie obiekty
    /// </summary>
    class Free : Terrain
    {
        /// <summary>
        /// Tworzy nowy wolny teren
        /// </summary>
        public Free()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Free"));
            _accessibility = Accessibility.Free;
        }
    }
}
