import { Button } from "../../components/ui/button";
import { Loader2 } from "lucide-react";

type SubmitButtonProps = {
  isCreating: boolean;
};

export function SubmitButton(
  { isCreating }: SubmitButtonProps,
) {
  return (    
    <Button
      type="submit"
      className="w-full"
      disabled={isCreating}
    >
      {isCreating ? (
        <>
          <Loader2 className="animate-spin mr-2 h-4 w-4" />
          Working...
        </>
      ) : (
        "Create"
      )}
    </Button>
  );
}