# Product matching

## Frontend

- ☑️ Implement confirmation button for a product match. This button should allow users to confirm that the matched product is correct and should be saved to the receipt .
- Add ability to remove a matched product from the receipt preview. This feature should allow users to unmatch a product if they realize it was matched incorrectly.
- Add all matched products to the pantry when the user confirms the receipt. This should save all confirmed matched products to the user's pantry.
- Allow users to manually match products if the automatic matching fails. This will provide a fallback option for users who encounter issues with the automatic matching process.
- Match products without barcodes. The system should be able to match products based on their names even if they don't have barcodes.


## Backend

- Grocery store receipt reader should not use product dto reader. The receipt reader should use a different model.