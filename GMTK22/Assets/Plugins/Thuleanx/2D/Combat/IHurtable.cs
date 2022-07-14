namespace Thuleanx2D.Combat {
	public interface IHurtable {
		bool CanTakeHit();
		void ApplyHit(IHit hit);
	}
}