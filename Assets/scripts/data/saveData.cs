

[System.Serializable]
public class SaveData
{
    public int night;
    public bool star1;
    public bool star2;
    public bool star3;

    public SaveData() {
        night = 1;
        star1 = false;
        star2 = false;
        star3 = false;
    }

    public SaveData(SaveData data) {
        night = data.night;
        star1 = data.star1;
        star2 = data.star2;
        star3 = data.star3;
    }
}