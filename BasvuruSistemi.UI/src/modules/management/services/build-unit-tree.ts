import { Unit } from "../models/unit-node.model";

export function buildUnitTree(units: Unit[]): Unit[] {
  const unitMap = new Map<string, Unit>();

  units.forEach((unit) => {
    unit.children = [];
    unitMap.set(unit.id, unit);
  });

  const tree: Unit[] = [];

  units.forEach((unit) => {
    if (unit.parentId) {
      const parent = unitMap.get(unit.parentId);
      if (parent) {
        parent.children?.push(unit);
      }
    } else {
      tree.push(unit);
    }
  });

  return tree;
}
