import React, { useState, useEffect } from "react"
import { useParams, useNavigate } from "react-router-dom"
import axios from "../api/axios"

const EditPost = () => {
  const { id } = useParams()
  const navigate = useNavigate()
  const [content, setContent] = useState("")
  const [image, setImage] = useState(null)
  const [username, setUsername] = useState("")
  const [error, setError] = useState("")
  const [success, setSuccess] = useState("")

  useEffect(() => {
    const fetchPost = async () => {
      try {
        const response = await axios.get(`/post/${id}`)
        const post = response.data
        setContent(post.content)
        setUsername(post.username)
        if (post.imageData) {
          document.getElementById(
            "imagePreview"
          ).src = `data:image/jpeg;base64,${post.imageData}`
        }
      } catch (error) {
        setError("Failed to fetch post data.")
      }
    }

    fetchPost()
  }, [id])

  const handleImageChange = (event) => {
    const file = event.target.files[0]
    if (file) {
      setImage(file)
      const reader = new FileReader()
      reader.onload = (e) => {
        document.getElementById("imagePreview").src = e.target.result
      }
      reader.readAsDataURL(file)
    }
  }

  const handleSubmit = async (event) => {
    event.preventDefault()
    setError("")
    setSuccess("")

    const formData = new FormData()
    formData.append("Content", content)
    formData.append("Username", username)
    if (image) {
      formData.append("Image", image)
    }

    try {
      const response = await axios.put(`/post/${id}`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      if (response.data.success) {
        setSuccess("Post updated successfully!")
        setTimeout(() => {
          navigate("/")
        }, 1500)
      } else {
        setError(response.data.message)
      }
    } catch (error) {
      setError("An error occurred while updating the post.")
    }
  }

  return (
    <div className="container mt-5">
      <h2>Edit Post</h2>
      {error && <div className="alert alert-danger">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}
      <form onSubmit={handleSubmit} className="create-post-container d-flex">
        {/* Left side for image preview and selecting image */}
        <div className="left-column flex-grow-1">
          <img
            className="image-placeholder"
            id="imagePreview"
            alt="Selected"
            style={{ width: "100%", height: "auto", textAlign: "center" }}
          />
          <label
            htmlFor="fileUpload"
            className="custom-file-upload btn btn-primary mt-3"
          >
            Select Image
          </label>
          <input
            type="file"
            accept="image/*"
            className="form-control-file"
            name="image"
            id="fileUpload"
            style={{ display: "none" }}
            onChange={handleImageChange}
          />
        </div>

        {/* Right side for writing the note */}
        <div className="right-column flex-grow-1">
          <textarea
            name="Content"
            id="noteInput"
            className="form-control"
            placeholder="Write note here..."
            value={content}
            onChange={(e) => setContent(e.target.value)}
            required
          ></textarea>
          <button type="submit" className="postbutton btn btn-primary mt-3">
            Update
          </button>
        </div>
      </form>
    </div>
  )
}

export default EditPost
