import { atom } from "jotai";
import { type PageKey, PageKeys } from "@/Help/pages";

export const SelectedPageKeyAtom = atom<PageKey>(PageKeys[0]);
