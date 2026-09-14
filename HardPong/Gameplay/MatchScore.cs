namespace HardPong;

// Puntuacion y reglas de victoria de la partida.
internal class MatchScore
{
    private const int WinningScore = 10;

    public int Player1 { get; private set; }
    public int Player2 { get; private set; }
    public PlayerId LastPointWinner { get; private set; }
    public PlayerId MatchWinner { get; private set; }

    public MatchScore()
    {
        Reset();
    }

    public void AwardPoint(PlayerId scorer)
    {
        if (scorer == PlayerId.None || MatchWinner != PlayerId.None)
            return;

        LastPointWinner = scorer;
        if (scorer == PlayerId.Player1)
            ++Player1;
        else
            ++Player2;

        if (Player1 >= WinningScore)
            MatchWinner = PlayerId.Player1;
        else if (Player2 >= WinningScore)
            MatchWinner = PlayerId.Player2;
    }

    // Limpia el punto disputado; conserva marcador y ganador del partido.
    public void StartNextRound()
    {
        LastPointWinner = PlayerId.None;
    }

    public void Reset()
    {
        Player1 = 0;
        Player2 = 0;
        LastPointWinner = PlayerId.None;
        MatchWinner = PlayerId.None;
    }
}
