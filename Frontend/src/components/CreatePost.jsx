import React from "react"

function CreatePost() {
  return (
    <form
      asp-action="CreatePost"
      method="post"
      enctype="multipart/form-data"
      class="create-post-container d-flex"
    >
      {/* <!-- Left side for image preview and selecting image --> */}
      <div class="left-column flex-grow-1">
        <div class="image-placeholder" id="imagePreview">
          <span id="imageText">Display selected image</span>
        </div>
        <label for="fileUpload" class="custom-file-upload btn btn-primary mt-3">
          Select Image
        </label>
        <input
          type="file"
          accept="image/*"
          class="form-control-file"
          name="image"
          id="fileUpload"
          style={{ display: "none" }}
          onchange="previewImage(event)"
        />
      </div>

      {/* <!-- Right side for writing the note --> */}
      <div class="right-column flex-grow-1">
        <input
          class="d-none"
          type="text"
          name="Username"
          placeholder="Username"
          value=""
          required
        />
        <textarea
          name="Content"
          id="noteInput"
          class="form-control"
          placeholder="Write note here..."
          required
        ></textarea>
        <button type="submit" class="postbutton btn btn-primary mt-3">
          Post
        </button>
      </div>
    </form>
  )
}

export default CreatePost
