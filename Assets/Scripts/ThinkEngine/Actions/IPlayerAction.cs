using Players;

namespace ThinkEngine.Actions {
    public interface IPlayerAction {
        
        int PlayerID { get; set; } 
        Player Player { get; }
    }
}
