import { atom } from "jotai";
import { type PageKey, PageKeys } from "../page";

export const SelectedPageKeyAtom = atom<PageKey>(PageKeys[0]);
