import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "../../components/ui/popover";
import {
  Command,
  CommandGroup,
  CommandItem,
  CommandList,
} from "../../components/ui/command";
import { Check, ChevronsUpDown } from "lucide-react";
import { Button } from "../../components/ui/button";
import { useState } from "react";
import { cn } from "../../lib/utils";
import { UseFormReturn } from "react-hook-form";
import { z } from "zod";
import { formSchema, frameworks } from "../../lib/form-schema";

type FormSchema = z.infer<typeof formSchema>;

type FrameworkSelectFieldProps = {
  form: UseFormReturn<FormSchema>;
};

export function FrameworkSelectField({ form }: FrameworkSelectFieldProps) {
  const [open, setOpen] = useState(false);

  return (
    <FormField
      control={form.control}
      name="frameworkVersion"
      render={({ field }) => (
        <FormItem className="flex flex-col">
          <FormLabel>Framework Version</FormLabel>
          <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
              <FormControl>
                <Button
                  variant="outline"
                  role="combobox"
                  aria-expanded={open}
                  className="w-[200px] justify-between"
                >
                  {field.value
                    ? frameworks.find((f) => f.value === field.value)?.label
                    : "Select framework..."}
                  <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                </Button>
              </FormControl>
            </PopoverTrigger>
            <PopoverContent className="w-[200px] p-0">
              <Command>
                <CommandList>
                  <CommandGroup>
                    {frameworks.map((framework) => {
                      const isSelected = field.value === framework.value;
                      return (
                        <CommandItem
                          key={framework.value}
                          value={framework.value}
                          onSelect={() => {
                            form.setValue("frameworkVersion", framework.value, {
                              shouldValidate: true,
                            });
                            setOpen(false);
                          }}
                        >
                          {framework.label}
                          <Check
                            style={{ opacity: isSelected ? 1 : 0 }}
                            className={cn("ml-auto h-4 w-4")}
                          />
                        </CommandItem>
                      );
                    })}
                  </CommandGroup>
                </CommandList>
              </Command>
            </PopoverContent>
          </Popover>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
