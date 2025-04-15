import { FieldValues, Path, UseFormReturn } from "react-hook-form";
import { ChecklistItem } from "./checklist-Item";

export type CheckListFieldProps<T extends FieldValues> = {
  form: UseFormReturn<T>;
  name: Path<T>;
  label?: string;
  description?: string;
  items: ChecklistItem[];
};