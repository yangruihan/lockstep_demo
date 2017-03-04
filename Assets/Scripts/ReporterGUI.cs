using UnityEngine;

public class ReporterGUI : MonoBehaviour
{
    Reporter reporter;
    void Awake()
    {
        DontDestroyOnLoad(this);

        reporter = FindObjectOfType<Reporter>();
        if (reporter != null)
        {
            reporter.OnReporterClose = OnReportClose;
        }
        this.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (reporter != null)
        {
            reporter.OnReporterClose = null;
        }
    }

    private void OnReportClose()
    {
        SwitchOff();
    }

    public void SwicthOn()
    {
        if (reporter != null)
        {
            gameObject.SetActive(true);
            reporter.show = true;
        }
    }

    private void SwitchOff()
    {
        if (gameObject != null)
            gameObject.SetActive(false);
    }

    void OnGUI()
    {
        if (reporter != null)
            reporter.OnGUIDraw();
    }
}
