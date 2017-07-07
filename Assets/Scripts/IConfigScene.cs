using System.Xml.Linq;

public interface IConfigScene {

	XElement remoteConfig { get; set;}
	XElement localConfig { get; set;}
}
